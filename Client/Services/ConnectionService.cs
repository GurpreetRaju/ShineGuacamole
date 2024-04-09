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
    }
}
