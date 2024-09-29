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

using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShineGuacamole.Services;

namespace ShineGuacamole.Components.Layout
{
    /// <summary>
    /// The main layout.
    /// </summary>
    public partial class MainLayout : IDisposable
    {
        private MudTheme _theme = new MudTheme();

        /// <summary>
        /// The user preferences.
        /// </summary>
        [Inject]
        private UserPreferences UserPreferences { get; set; }

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            UserPreferences.StateHasChanged += HandleStateHasChanged;
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            UserPreferences.StateHasChanged -= HandleStateHasChanged;
        }

        /// <summary>
        /// Handles the user preferences state changed.
        /// </summary>
        private void HandleStateHasChanged(object sender, EventArgs args)
        {
            InvokeAsync(StateHasChanged);
        }
    }
}
