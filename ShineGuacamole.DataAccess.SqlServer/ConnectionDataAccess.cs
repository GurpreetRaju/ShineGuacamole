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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShineGuacamole.DataAccess.SqlServer.Models;
using ShineGuacamole.Library.DataAccess;
using ShineGuacamole.Shared.Models;

namespace ShineGuacamole.DataAccess.SqlServer
{
    /// <summary>
    /// Provides access to the connection data.
    /// </summary>
    public class ConnectionDataAccess : IConnectionsDataAccess
    {
        private readonly ShineGuacContext _context;
        private readonly ILogger<ConnectionDataAccess> _logger;

        /// <summary>
        /// Initialize the connection data access.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger.</param>
        public ConnectionDataAccess(ShineGuacContext context, ILogger<ConnectionDataAccess> logger) 
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ConnectionInfo> GetConnectionInfo(string connectionId)
        {
            _logger.LogDebug($"Get connection. Connection Id: {connectionId}");

            return ToConnectionInfo(await FindConnection(connectionId));
        }

        /// <inheritdoc/>
        public async Task<IConnectionProperties> GetConnectionProperties(string connectionId)
        {
            _logger.LogDebug($"Get connection properties. Connection Id: {connectionId}");

            var connection = await FindConnection(connectionId);
            if (connection?.Properties == null) throw new InvalidOperationException($"{connectionId} is not a valid connection");

            try
            {
                return JsonConvert.DeserializeObject<IConnectionProperties>(connection.Properties);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to convert connection {connectionId} properties. {ex}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<(ConnectionInfo Info, string PropertiesJson)> GetConnectionWithProperties(string connectionId)
        {
            _logger.LogDebug($"Get connection with properties. Connection Id: {connectionId}");

            var connection = await FindConnection(connectionId);
            if (connection == null) throw new InvalidOperationException($"{connectionId} is not a valid connection");

            var connectionInfo = ToConnectionInfo(connection);
            
            return (connectionInfo, connection.Properties);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ConnectionInfo>> GetConnections(string userId)
        {
            _logger.LogDebug($"Get connections. User Id: {userId}");

            return await _context.Connections.AsQueryable()
                .Where(c => c.UserId == userId)
                .Select(c => ToConnectionInfo(c)).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task SaveConnection(string userId, ConnectionInfo connection, IConnectionProperties properties)
        {
            _logger.LogDebug($"Add connection. Connection: {connection}");

            if (connection == null) throw new ArgumentNullException(nameof(connection));

            try 
            { 
                var json = properties == null ? string.Empty : JsonConvert.SerializeObject(properties);

                if (connection.Id == null)
                {
                    await _context.Connections.AddAsync(new Connection
                    {
                        Name = connection.Name,
                        Image = connection.Image,
                        Type = connection.Type.ToString(),
                        UserId = userId,
                        Properties = json
                    });
                }
                else
                {
                    var dbConnection = await FindConnection(connection.Id);
                    if (dbConnection == null) throw new InvalidOperationException($"{connection.Id} is not a valid connection.");

                    dbConnection.Name = connection.Name;
                    dbConnection.Image = connection.Image;
                    dbConnection.Type = connection.Type.ToString();
                    dbConnection.UserId = userId;
                    dbConnection.Properties = json;
                }
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save connection {connection.Name} properties. {ex}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task RemoveConnection(string userId, string connectionId)
        {
            _logger.LogDebug($"Remove connection. ConnectionId: {connectionId}, UserId: {userId}");

            if (connectionId == null) throw new ArgumentNullException(nameof(connectionId));

            try
            {
                var dbConnection = await FindConnection(connectionId);
                if (dbConnection == null) throw new InvalidOperationException($"{connectionId} is not a valid connection.");

                _context.Connections.Remove(dbConnection);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to remove connection {connectionId}. {ex}");
                throw;
            }
        }

        /// <summary>
        /// Gets the connection with given connection id.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <returns></returns>
        private async Task<Connection> FindConnection(string connectionId)
        {
            var connection = await _context.Connections.FindAsync(Guid.Parse(connectionId));
            if (connection == null) throw new InvalidOperationException($"{connectionId} is not a valid connection");

            return connection;
        }

        /// <summary>
        /// Converts Connection to ConnectionInfo.
        /// </summary>
        /// <param name="connection">The connection object.</param>
        /// <returns></returns>
        private static ConnectionInfo ToConnectionInfo(Connection connection)
        {
            return new ConnectionInfo
            {
                Id = connection.ConnectionId.ToString(),
                Image = connection.Image,
                Name = connection.Name,
                Type = Enum.Parse<ConnectionType>(connection.Type)
            };
        }
    }
}
