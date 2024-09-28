using Microsoft.AspNetCore.Components;
using ShineGuacamole.Services;
using ShineGuacamole.Shared;

namespace ShineGuacamole.Components.Pages
{
    /// <summary>
    /// Base class for pages.
    /// </summary>
    public partial class PageView
    {
        /// <summary>
        /// Appbar content provider.
        /// </summary>
        [Inject]
        private IAppBarContentProvider AppBarContentProvider { get; set; }

        /// <summary>
        /// The logger.
        /// </summary>
        [Inject]
        private ILogger<PageView> Logger { get; set; }

        /// <summary>
        /// Whether to show the loading screen.
        /// </summary>
        [Parameter]
        public bool Loading { get; set; }

        /// <summary>
        /// Page title.
        /// </summary>
        [Parameter]
        public string PageTitle { get; set; }

        /// <summary>
        /// Provides actions for this page.
        /// </summary>
        [Parameter]
        public IEnumerable<ActionConfig> PageActions { get; set; }

        /// <summary>
        /// Content for this page.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            try
            {
                base.OnInitialized();

                AppBarContentProvider.PageTitle = PageTitle;
                AppBarContentProvider.SetPageActions(PageActions ?? []);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to initialize. {ex}");
            }
        }
    }
}
