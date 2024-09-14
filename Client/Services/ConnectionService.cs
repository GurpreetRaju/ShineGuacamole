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
using System.Net.Http.Json;

namespace ShineGuacamole.Client.Services
{
    /// <summary>
    /// The connection service.
    /// </summary>
    public class ConnectionService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes the connection service.
        /// </summary>
        /// <param name="httpClient"></param>
        public ConnectionService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets the connections.
        /// </summary>
        /// <returns></returns>
        public async Task<ConnectionInfo[]> GetConnections()
        {
            return await _httpClient.GetFromJsonAsync<ConnectionInfo[]>("/connections");
        }

        /// <summary>
        /// Save the connection.
        /// </summary>
        /// <returns></returns>
        public async Task SaveConnection(ConnectionInfoExtended connection)
        {
            await _httpClient.PostAsJsonAsync("/save-connection", connection);
        }
    }
}
