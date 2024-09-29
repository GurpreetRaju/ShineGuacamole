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
