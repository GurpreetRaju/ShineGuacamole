using System.Net.WebSockets;

namespace ShineGuacamole.Services.Models
{
    public class ConnectionRequest
    {
        /// <summary>
        /// Connection Type parameter name.
        /// </summary>
        public const string Type = nameof(Type);

        /// <summary>
        /// Initializes the connection request.
        /// </summary>
        public ConnectionRequest(Dictionary<string, string> arguments, TaskCompletionSource completeTokenSource, WebSocket socket)
        {
            Id = Guid.NewGuid();
            Arguments = arguments;
            Complete = completeTokenSource;
            Socket = socket;
        }

        /// <summary>
        /// The Connection Identifier.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Connection Arguments.
        /// </summary>
        public Dictionary<string, string> Arguments { get; }

        /// <summary>
        /// The token source that will be set when the connection is closed.
        /// </summary>
        public TaskCompletionSource Complete { get; }

        /// <summary>
        /// The web socket.
        /// </summary>
        public WebSocket Socket { get; }
    }
}
