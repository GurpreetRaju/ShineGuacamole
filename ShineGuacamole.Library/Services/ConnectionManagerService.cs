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

            var connectionConfiguration = await _connectionsDataAccess.GetConnectionDetails(connectionId);

            return connectionConfiguration;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ConnectionInfo>> GetConnections(string userId)
        {
            _logger.LogInformation(nameof(GetConnectionConfiguration) + $" - Called.");

            var connections = await _connectionsDataAccess.GetConnections(userId);

            return connections;
        }

        /// <inheritdoc/>
        public async Task SaveConnection(string userId, ConnectionInfo connection, Dictionary<string, string> connectionConfiguration)
        {
            _logger.LogInformation(nameof(SaveConnection) + $" - Called.");

            await _connectionsDataAccess.SaveConnection(userId, connection, connectionConfiguration);
        }

        /// <inheritdoc/>
        public async Task RemoveConnection(string userId, string connectionId)
        {
            _logger.LogInformation(nameof(RemoveConnection) + $" - Called.");

            await _connectionsDataAccess.RemoveConnection(userId, connectionId);
        }
    }
}
