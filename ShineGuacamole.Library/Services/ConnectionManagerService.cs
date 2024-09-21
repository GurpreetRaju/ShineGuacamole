#region Copyright
//
// Copyright 2024 Gurpreet Raju
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

using Microsoft.Extensions.Logging;
using ShineGuacamole.Shared.Models;
using ShineGuacamole.Services.Interfaces;
using ShineGuacamole.Library.DataAccess;
using Newtonsoft.Json;

namespace ShineGuacamole.Services
{
    /// <summary>
    /// A service to manage remote connections.
    /// </summary>
    public class ConnectionManagerService : IConnectionManagerService
    {
        private readonly ILogger _logger;
        private readonly IConnectionsDataAccess _connectionsDataAccess;

        /// <summary>
        /// Initializes the connection manager service.
        /// </summary>
        public ConnectionManagerService(ILogger<ConnectionManagerService> logger,
            IConnectionsDataAccess connectionsDataAccess)
        {
            _logger = logger;
            _connectionsDataAccess = connectionsDataAccess;
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, string>> GetConnectionConfiguration(string connectionId)
        {
            _logger.LogInformation(nameof(GetConnectionConfiguration) + $" - Called. Id: {connectionId}");

            var connectionWithProps = await _connectionsDataAccess.GetConnectionWithProperties(connectionId);

            try
            {
                var connectionConfiguration = JsonConvert.DeserializeObject<Dictionary<string, string>>(connectionWithProps.PropertiesJson);

                connectionConfiguration.Add("type", connectionWithProps.Info.Type.ToString().ToLowerInvariant());

                return connectionConfiguration;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get the properties of connection {connectionId}. {ex}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IConnectionProperties> GetConnectionProperties(string connectionId)
        {
            _logger.LogInformation(nameof(GetConnectionProperties) + $" - Called. Id: {connectionId}");

            return await _connectionsDataAccess.GetConnectionProperties(connectionId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ConnectionInfo>> GetConnections(string userId)
        {
            _logger.LogInformation(nameof(GetConnectionConfiguration) + $" - Called.");

            var connections = await _connectionsDataAccess.GetConnections(userId);

            return connections;
        }

        /// <inheritdoc/>
        public async Task SaveConnection(string userId, ConnectionInfo connection, IConnectionProperties properties)
        {
            _logger.LogInformation(nameof(SaveConnection) + $" - Called.");

            await _connectionsDataAccess.SaveConnection(userId, connection, properties);
        }

        /// <inheritdoc/>
        public async Task RemoveConnection(string userId, string connectionId)
        {
            _logger.LogInformation(nameof(RemoveConnection) + $" - Called.");

            await _connectionsDataAccess.RemoveConnection(userId, connectionId);
        }
        
        /// <inheritdoc/>
        public async Task<(ConnectionInfo Connection, IConnectionProperties Properties)> GetConnection(string connectionId)
        {
            _logger.LogInformation(nameof(GetConnection) + $" - Called. Id: {connectionId}");

            var result = await _connectionsDataAccess.GetConnectionWithProperties(connectionId);
            IConnectionProperties properties;
            try
            {
                properties = JsonConvert.DeserializeObject<ConnectionProperties>(result.PropertiesJson);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to parse the connection properties for {connectionId}. {ex}");
                throw;
            }

            return (result.Info, properties);
        }
    }
}
