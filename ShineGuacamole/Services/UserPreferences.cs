namespace ShineGuacamole.Services
{
    /// <summary>
    /// Stores the user preferences.
    /// </summary>
    public class UserPreferences
    {
        private readonly Guid _id;
        private readonly ILogger<UserPreferences> _logger;

        /// <summary>
        /// The event raised when a setting is changed.
        /// </summary>
        public event EventHandler<EventArgs> StateHasChanged;

        public UserPreferences(ILogger<UserPreferences> logger)
        {
            _id = Guid.NewGuid();
            _logger = logger;

            logger.LogInformation($"New preferences: {_id}");
        }

        /// <summary>
        /// The theme dark mode.
        /// </summary>
        public bool DarkMode { get; set; }

        /// <summary>
        /// Toggle the theme mode.
        /// </summary>
        public void ToggleThemeMode()
        {
            _logger.LogInformation($"ID: {_id} - Old Dark Mode: {DarkMode}");

            DarkMode = !DarkMode;

            StateHasChanged?.Invoke(this, EventArgs.Empty);

            _logger.LogInformation($"ID: {_id} - New Dark Mode: {DarkMode}");
        }
    }
}
