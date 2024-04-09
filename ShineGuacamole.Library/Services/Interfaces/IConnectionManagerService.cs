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
        /// Gets the connections.
        /// </summary>
        /// <returns>An awaitable task that returns collection of <seealso cref="ConnectionInfo"/>.</returns>
        Task<IEnumerable<ConnectionInfo>> GetConnections();
    }
}
