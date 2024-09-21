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
using System;

namespace ShineGuacamole.Library.DataAccess
{
    /// <summary>
    /// Interface for Connection Data.
    /// </summary>
    public interface IConnectionsDataAccess
    {
        /// <summary>
        /// Gets connection.
        /// </summary>
        /// <param name="connectionId">The unique connection identifier.</param>
        /// <returns>The connection.</returns>
        Task<ConnectionInfo> GetConnectionInfo(string connectionId);

        /// <summary>
        /// Gets connection details.
        /// </summary>
        /// <param name="connectionId">The unique connection identifier.</param>
        /// <returns>Dictionary containing the connection properties.</returns>
        Task<IConnectionProperties> GetConnectionProperties(string connectionId);

        /// <summary>
        /// Gets connections infos filtered by the given parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A collection of connection infos</returns>
        Task<IEnumerable<ConnectionInfo>> GetConnections(string userId);

        /// <summary>
        /// Save a connection.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="connection">The connection information.</param>
        /// <param name="properties">The connection properties.</param>
        /// <returns></returns>
        Task SaveConnection(string userId, ConnectionInfo connection, IConnectionProperties properties);

        /// <summary>
        /// Remove a connection.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <returns></returns>
        Task RemoveConnection(string userId, string connectionId);

        /// <summary>
        /// Gets the connection and its properties
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <returns></returns>
        Task<(ConnectionInfo Info, string PropertiesJson)> GetConnectionWithProperties(string connectionId);
    }
}
