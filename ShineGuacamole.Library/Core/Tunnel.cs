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
#endregion

using ShineGuacamole.Models;
using Serilog;
using System.Net.Sockets;

namespace ShineGuacamole.Core
{
    public sealed class Tunnel
    {
        private readonly ClientSocket _client;
        private readonly TaskCompletionSource _complete;
        private readonly Connection _connection;
        private readonly CancellationToken _shutdownToken;
        private readonly GuacdSocket _guacd;
        private readonly Guid _id;
        private readonly object _shutdownLock;
        private bool _shutdownTriggered;

        public Tunnel(Guid id, Connection connection, ClientSocket client, GuacdSocket guacd, TaskCompletionSource complete, CancellationToken shutdownToken)
        {
            _id = id;
            _connection = connection;
            _client = client;
            _guacd = guacd;
            _complete = complete;
            _shutdownToken = shutdownToken;
            _shutdownLock = new object();
            _shutdownTriggered = false;
        }

        public async Task CloseAsync(Exception exception)
        {
            _guacd.Close();

            if (exception?.Message != null)
            {
                await _client.SendAsync($"5.error,{exception.Message.Length}.{exception.Message};");
            }

            await _client.CloseAsync();
            _complete.TrySetResult();
        }

        public async Task OpenAsync()
        {
            try
            {
                await _guacd.OpenConnectionAsync(_connection);
            }
            catch (Exception ex)
            {
                Log.Error("[{Id}] Error during handshake phase: {Message}.", _id, ex.Message);
                Log.Information("[{Id}] Closing connection...", _id);

                await CloseAsync(ex);
                return;
            }

            _ = Task.Run(async () =>
            {
                while (!_shutdownToken.IsCancellationRequested)
                {          
                    var message = await _client.ReceiveAsync();

                    if (message.Contains("10.disconnect;"))
                    {
                        break;
                    }

                    await _guacd.SendAsync(message);
                }
            }).ContinueWith(t => HandleShutdown(t), CancellationToken.None, TaskContinuationOptions.RunContinuationsAsynchronously, TaskScheduler.Default);


            _ = Task.Run(async () =>
            {
                while (!_shutdownToken.IsCancellationRequested)
                {
                    var message = await _guacd.ReceiveAsync();

                    if (message.Contains("10.disconnect;"))
                    {
                        break;
                    }

                    await _client.SendAsync(message);
                }
            }).ContinueWith(t => HandleShutdown(t), CancellationToken.None, TaskContinuationOptions.RunContinuationsAsynchronously, TaskScheduler.Default);
        }

        private async Task HandleShutdown(Task t)
        {
            lock (_shutdownLock)
            {
                if (_shutdownTriggered)
                {
                    return;
                }
                else
                {
                    _shutdownTriggered = true;
                }
            }

            if (t.Exception != null)
            {
                if (t.Exception.InnerException is SocketException ex && ex.SocketErrorCode == SocketError.OperationAborted)
                {
                    Log.Warning("[{Id}] Socket operation has been aborted.", _id);
                }
                else
                {
                    Log.Error("[{Id}] Sockets faulted unexpectedly: {Message}", _id, t.Exception.Message);
                }
            }

            Log.Information("[{Id}] Closing connection...", _id);

            await CloseAsync(t.Exception);
        }
    }
}
