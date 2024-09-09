#region Copyright
//
// Copyright 2022 ManuelExpunged
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// The code is based of ConnectionProcessorService.
#endregion

using ShineGuacamole.Core;
using ShineGuacamole.Models;
using ShineGuacamole.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using ShineGuacamole.Services.Models;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;

namespace ShineGuacamole.Services
{
    /// <summary>
    /// Background service to handle remote connections.
    /// </summary>
    public class RemoteConnectionService : BackgroundService
    {
        private readonly ManualResetEvent _idle;
        private readonly ConcurrentQueue<ConnectionRequest> _pendingConnections;
        private readonly SemaphoreSlim _processing;
        private readonly CancellationTokenSource _shutdownTokenSource;
        private readonly ClientOptions _clientOptions;
        private readonly GuacdOptions _guacdOptions;

        /// <summary>
        /// Initializes the Remote Connection Service.
        /// </summary>
        /// <param name="clientOptions"></param>
        /// <param name="ShineGuacamoleOptions"></param>
        /// <param name="guacdOptions"></param>
        public RemoteConnectionService(IOptions<ClientOptions> clientOptions,
            IOptions<GuacdOptions> guacdOptions)
        {
            _clientOptions = clientOptions.Value;
            _guacdOptions = guacdOptions.Value;

            _pendingConnections = new ConcurrentQueue<ConnectionRequest>();
            _idle = new ManualResetEvent(false);
            _processing = new SemaphoreSlim(1, 1);
            _shutdownTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Add a remote connection.
        /// </summary>
        /// <param name="request">The new connection request.</param>
        /// <returns>An awaitable task.</returns>
        public async Task AddAsync(ConnectionRequest request)
        {
            // wait until previous sockets have finished processing
            await _processing.WaitAsync(_shutdownTokenSource.Token);

            try
            {
                _pendingConnections.Enqueue(request);
                
                // signal wake up
                _idle.Set();
            }
            finally
            {
                // sockets added, allow processing or adding of more sockets
                _processing.Release();
            }
        }

        /// <summary>
        /// Stop the service.
        /// </summary>
        /// <param name="stoppingToken">The cancellation token.</param>
        /// <returns>An awaitable task.</returns>
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _shutdownTokenSource.Cancel();

            _idle.Set();

            await Task.Delay(5000, CancellationToken.None);

            await base.StopAsync(stoppingToken);
        }

        /// <summary>
        /// Execute.
        /// </summary>
        /// <param name="stoppingToken">The cancellation token set when the <see cref="StopAsync(CancellationToken)"/> is called.</param>
        /// <returns>An awaitable task.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            while (!stoppingToken.IsCancellationRequested)
            {
                _idle.WaitOne();
                await _processing.WaitAsync(_shutdownTokenSource.Token);

                try
                {
                    while (_pendingConnections.TryDequeue(out var pendingConnection))
                    {
                        Connection connection;
                        IPEndPoint endpoint;

                        try
                        {
                            connection = GetConnectionConfiguration(pendingConnection);
                            endpoint = GetProxyEndPoint();
                        }
                        catch (Exception ex)
                        {
                            Log.Error("[{Id}] {Message}.", pendingConnection.Id, ex.Message);
                            Log.Information("[{Id}] Closing connection...", pendingConnection.Id);
                            await pendingConnection.Socket.CloseAsync(WebSocketCloseStatus.InternalServerError, string.Empty, CancellationToken.None);
                            pendingConnection.Complete.TrySetResult();
                            continue;
                        }

                        var client = new ClientSocket(pendingConnection.Id, pendingConnection.Socket, _shutdownTokenSource.Token);
                        var guacd = new GuacdSocket(pendingConnection.Id, endpoint, _shutdownTokenSource.Token);
                        var tunnel = new Tunnel(pendingConnection.Id, connection, client, guacd, pendingConnection.Complete, _shutdownTokenSource.Token); ;
                        await tunnel.OpenAsync();
                    }

                    _idle.Reset();
                }
                finally
                {
                    _processing.Release();
                }
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Gets the connection coniguration.
        /// </summary>
        /// <param name="pendingConnection">The connection request.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual Connection GetConnectionConfiguration(ConnectionRequest pendingConnection)
        {
            try
            {
                Log.Information("[{Id}] Building connection configuration...", pendingConnection.Id);
                
                var connectionTypeKey = pendingConnection.Arguments.Keys.FirstOrDefault(k => k.Equals("Type", StringComparison.InvariantCultureIgnoreCase));
                if (string.IsNullOrWhiteSpace(connectionTypeKey))
                    throw new InvalidOperationException("The connection type is not specified.");
                
                var connection = new Connection();
                connection.Type = pendingConnection.Arguments[connectionTypeKey].ToLowerInvariant();

                if (_clientOptions.DefaultArguments?.ContainsKey(connection.Type) == true) {
                    foreach (var arg in _clientOptions.DefaultArguments[connection.Type])
                    {
                        if (!connection.Arguments.ContainsKey(arg.Key))
                        {
                            connection.Arguments.Add(arg.Key, arg.Value);
                        }
                    }
                }

                foreach (var key in pendingConnection.Arguments.Keys)
                {
                    if (key.Equals(connectionTypeKey) 
                        && string.IsNullOrWhiteSpace(pendingConnection.Arguments[key])) continue;

                    connection.Arguments[key] = pendingConnection.Arguments[key];
                }

                Log.Debug("[{Id}] Connection configuration: {@Connection}", pendingConnection.Id, connection);

                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to configure connection: {ex.Message}");
            }
        }

        private IPEndPoint GetProxyEndPoint()
        {
            try
            {
                if (!IPAddress.TryParse(_guacdOptions.Hostname, out IPAddress? ip) || ip == null)
                {
                    ip = Dns.GetHostAddresses(_guacdOptions.Hostname).First(x => x.AddressFamily == AddressFamily.InterNetwork);
                }

                return new IPEndPoint(ip, _guacdOptions.Port);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to configure endpoint: {ex.Message}");
            }
        }
    }
}
