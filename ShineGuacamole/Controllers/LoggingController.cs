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

namespace ShineGuacamole.Server.Controllers
{
    /// <summary>
    /// The Logging Controller handles the log requests.
    /// </summary>
    [ApiController]
    public class LoggingController : Controller
    {
        private readonly ILogger<LoggingController> _logger;

        /// <summary>
        /// Initialized the logging controller.
        /// </summary>
        /// <param name="logger"></param>
        public LoggingController(ILogger<LoggingController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles the logging.
        /// </summary>
        /// <returns></returns>
        [HttpPost("/logging-event")]
        public void Log([FromBody] LogEvent[] logEvents)
        {
            foreach (var logEvent in logEvents)
            {
                _logger.LogInformation("Message: {message}", logEvent.RenderedMessage);
            }
        }        
    }

    /// <summary>
    /// Represents the log event.
    /// </summary>
    public class LogEvent
    {
        /// <summary>
        /// The timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The level.
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// The message.
        /// </summary>
        public string RenderedMessage { get; set; }
    }
}
