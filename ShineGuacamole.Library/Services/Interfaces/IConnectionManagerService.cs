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

using ShineGuacamole.Shared.Models;

namespace ShineGuacamole.Services.Interfaces
{
    /// <summary>
    /// Manages the remote connection configurations.
    /// </summary>
    public interface IConnectionManagerService
    {
        /// <summary>
        /// Gets the connection configuration for given connection identifier.
        /// </summary>
        /// <param name="connectionId">The connection identifier/string.</param>
        /// <returns>An awaitable task that returns connection arguments dictionary.</returns>
        Task<Dictionary<string, string>> GetConnectionConfiguration(string connectionId);

        /// <summary>
        /// Gets the connection properties for given connection identifier.
        /// </summary>
        /// <param name="connectionId">The connection identifier/string.</param>
        /// <returns>An awaitable task that returns connection arguments dictionary.</returns>
        Task<IConnectionProperties> GetConnectionProperties(string connectionId);

        /// <summary>
        /// Gets the connections.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>An awaitable task that returns collection of <seealso cref="ConnectionInfo"/>.</returns>
        Task<IEnumerable<ConnectionInfo>> GetConnections(string userId);

        /// <summary>
        /// Save a connection.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="connection">The connection info.</param>
        /// <param name="connectionProperties">The connection properties.</param>
        /// <returns></returns>
        Task SaveConnection(string userId, ConnectionInfo connection, IConnectionProperties connectionProperties);

        /// <summary>
        /// Remove a connection.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <returns></returns>
        Task RemoveConnection(string userId, string connectionId);

        /// <summary>
        /// Gets the connection with properties.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <returns></returns>
        Task<(ConnectionInfo Connection, IConnectionProperties Properties)> GetConnection(string connectionId);
    }
}
