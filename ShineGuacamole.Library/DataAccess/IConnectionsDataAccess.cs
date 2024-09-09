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
        /// Gets connection details.
        /// </summary>
        /// <param name="connectionId">The unique connection identifier.</param>
        /// <returns>Dictionary containing the connection properties.</returns>
        Task<Dictionary<string, string>> GetConnectionDetails(string connectionId);

        /// <summary>
        /// Gets connections infos filtered by the given parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A collection of connection infos</returns>
        Task<IEnumerable<ConnectionInfo>> GetConnections(string userId);
    }
}
