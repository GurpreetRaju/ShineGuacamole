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
