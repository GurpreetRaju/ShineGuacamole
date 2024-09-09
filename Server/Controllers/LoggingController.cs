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
