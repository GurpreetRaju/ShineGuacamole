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

using Microsoft.AspNetCore.Mvc;
using ShineGuacamole.Services;
using ShineGuacamole.Services.Interfaces;
using ShineGuacamole.Services.Models;
using ConnectionInfo = ShineGuacamole.Shared.Models.ConnectionInfo;

namespace ShineGuacamole.Server.Controllers
{
    /// <summary>
    /// The Remote Connection Controller handles the remote connection requests.
    /// </summary>
    [ApiController]
    public class RemoteConnectionController : ControllerBase
    {
        private readonly RemoteConnectionService _remoteConnectionService;
        private readonly IConnectionManagerService _connectionManagerService;
        private readonly ILogger<RemoteConnectionController> _logger;

        /// <summary>
        /// Initializes the Remote Connection Controller.
        /// </summary>
        public RemoteConnectionController(RemoteConnectionService remoteConnectionService, 
            IConnectionManagerService connectionManagerService,
            ILogger<RemoteConnectionController> logger)
        {
            _remoteConnectionService = remoteConnectionService;
            _connectionManagerService = connectionManagerService;
            _logger = logger;
        }

        /// <summary>
        /// Handles the connection request.
        /// </summary>
        /// <returns></returns>
        [Route("/connect")]
        public async Task Connect([FromQuery] string connectionString)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync("guacamole");

                var connectionArgs = await _connectionManagerService.GetConnectionConfiguration(connectionString);
                var complete = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

                _ = _remoteConnectionService.AddAsync(new ConnectionRequest(connectionArgs, complete, webSocket));

                await complete.Task;
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        [HttpGet("/connections")]
        public async Task<IActionResult> GetConnections()
        {
            _logger.LogInformation("Get connections request.");

            try
            {
                IEnumerable<ConnectionInfo> connections = await _connectionManagerService.GetConnections(null);

                return Ok(connections);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get connections list.");
                return Problem("Failed to get connections request.", statusCode: 500);
            }
        }
    }
}
