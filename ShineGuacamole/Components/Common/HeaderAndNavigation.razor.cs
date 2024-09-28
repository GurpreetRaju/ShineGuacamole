using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShineGuacamole.Shared;
using ShineGuacamole.Services;

namespace ShineGuacamole.Components.Common
{
    /// <summary>
    /// Header and navigation.
    /// </summary>
    public partial class HeaderAndNavigation : IDisposable
    {
        private bool _drawerOpen;
        private string _pageTitle;

        private static IEnumerable<ActionConfig> _navItems = [
            new ActionConfig{ Href = "/", Icon = Icons.Material.Filled.Home, Text = "Home", Match = NavLinkMatch.All },
            new ActionConfig{ Href = AppRoutes.Connections, Icon = Icons.Material.Filled.Apps, Text = "Connections", Match = NavLinkMatch.Prefix }
        ];

        /// <summary>
        /// The app bar content provider.
        /// </summary>
        [Inject]
        private IAppBarContentProvider AppBarContentProvider { get; set; }

        /// <summary>
        /// The user preferences.
        /// </summary>
        [Inject]
        private UserPreferences UserPreferences { get; set; }

        /// <summary>
        /// The app name.
        /// </summary>
        public virtual string ApplicationName { get; set; } = "Shine Guacamole";

        /// <summary>
        /// The current page title.
        /// </summary>
        public string Title => _pageTitle;

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            AppBarContentProvider.StateHasChanged += HandleStateHasChanged;
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        public void Dispose()
        {
            AppBarContentProvider.StateHasChanged -= HandleStateHasChanged;
        }

        /// <summary>
        /// Sets a page title.
        /// </summary>
        /// <param name="title"></param>
        public void SetPageTitle(string title)
        {
            _pageTitle = title;

            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Toggle drawer.
        /// </summary>
        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        /// <summary>
        /// Handles the <see cref="AppBarContentProvider.StateHasChanged"/>.
        /// </summary>
        private void HandleStateHasChanged()
        {
            InvokeAsync(StateHasChanged);
        }
    }
}
