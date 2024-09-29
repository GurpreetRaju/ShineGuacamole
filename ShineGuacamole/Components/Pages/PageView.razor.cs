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
