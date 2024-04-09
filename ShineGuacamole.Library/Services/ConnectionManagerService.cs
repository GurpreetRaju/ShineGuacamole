using Microsoft.Extensions.Logging;
using ShineGuacamole.Shared.Models;
using ShineGuacamole.Services.Interfaces;

namespace ShineGuacamole.Services
{
    /// <summary>
    /// A service to manage remote connections.
    /// </summary>
    public class ConnectionManagerService : IConnectionManagerService
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes the connection manager service.
        /// </summary>
        public ConnectionManagerService(ILogger<ConnectionManagerService> logger) 
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets the connection configuration for given connection identifier.
        /// </summary>
        /// <param name="connectionId">The connection identifier/string.</param>
        /// <returns>An awaitable task that returns connection arguments dictionary.</returns>
        public Task<Dictionary<string, string>> GetConnectionConfiguration(string connectionId)
        {
            _logger.LogInformation(nameof(GetConnectionConfiguration) + $" - Called. Id: {connectionId}");

            var connectionConfiguration = new Dictionary<string, string>
            {
                { "type", "RDP" },
                { "hostname", "0.0.0.0" },
                { "username", "user_name" },
                { "password", "user_password" }
            };

            return Task.FromResult(connectionConfiguration);
        }

        /// <summary>
        /// Gets the connections.
        /// </summary>
        /// <returns>An awaitable task that returns collection of <seealso cref="ConnectionInfo"/>.</returns>
        public async Task<IEnumerable<ConnectionInfo>> GetConnections()
        {
            _logger.LogInformation(nameof(GetConnectionConfiguration) + $" - Called.");

            var connections = new List<ConnectionInfo>()
            {
                new()
                {
                  Id = "some_id",
                  Name = "username",
                  Type = "RDP"
                }
            };

            return await Task.FromResult(connections);
        }
    }
}
