#region Copyright
//
// Copyright 2022 ManuelExpunged
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

using Serilog;
using System.Net.WebSockets;
using System.Text;

namespace ShineGuacamole.Core
{
    /// <summary>
    /// Client Socket.
    /// </summary>
    public sealed class ClientSocket
    {
        private readonly ArraySegment<byte> _buffer;
        private readonly Guid _id;
        private readonly StringBuilder _overflowBuffer;
        private readonly WebSocket _socket;
        private readonly CancellationToken _shutdownToken;

        /// <summary>
        /// Initializes the Client Socket.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="socket">The web socket.</param>
        /// <param name="shutdownToken">Shutdown cancellation token.</param>
        public ClientSocket(Guid id, WebSocket socket, CancellationToken shutdownToken)
        {
            _id = id;
            _socket = socket;
            _shutdownToken = shutdownToken;
            _buffer = new ArraySegment<byte>(new byte[1024]);
            _overflowBuffer = new StringBuilder();
        }

        /// <summary>
        /// Closes the socket.
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync()
        {
            try
            {
                if (_socket.State == WebSocketState.Open)
                {
                    await _socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, String.Empty, CancellationToken.None);
                    Log.Information("[{Id}] Client socket closed.", _id);
                }
            }
            catch (ObjectDisposedException)
            {
                Log.Warning("[{Id}] Client socket is already disposed.", _id);
            }
            catch (Exception ex)
            {
                Log.Error("[{Id}] Error while closing client socket: {Message}", _id, ex.Message);
            }
        }

        /// <summary>
        /// Receive messages from the client.
        /// </summary>
        /// <returns></returns>
        public async Task<string> ReceiveAsync()
        {
            WebSocketReceiveResult result;

            do
            {
                result = await _socket.ReceiveAsync(_buffer, _shutdownToken);
                if (result.Count > 0)
                {
                    _overflowBuffer.Append(Encoding.UTF8.GetString(_buffer[0..result.Count]));
                }
            }
            while (!result.EndOfMessage);

            var content = _overflowBuffer.ToString();
            var message = ProtocolHelper.ReadProtocolUntilLastDelimiter(content);

            if (message.index > 0)
            {
                _overflowBuffer.Remove(0, message.index);
            }

            return message.content;
        }

        /// <summary>
        /// Sends message to the client.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendAsync(string message)
        {
            Log.Debug("[{Id}] >>>G2C> {Message}", _id, message);

            var data = Encoding.UTF8.GetBytes(message);
            await _socket.SendAsync(new ArraySegment<byte>(data, 0, data.Length), WebSocketMessageType.Text, true, _shutdownToken);
        }
    }
}
