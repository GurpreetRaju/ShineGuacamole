using Microsoft.AspNetCore.Components;
using ShineGuacamole.Shared;
using ShineGuacamole.Shared.Models;

namespace ShineGuacamole.Client.Pages
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
        protected ILogger<PageView> Logger { get; set; }

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
