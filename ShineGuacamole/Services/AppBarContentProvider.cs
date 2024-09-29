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

using ShineGuacamole.Shared;

namespace ShineGuacamole.Services
{
    /// <summary>
    /// The service to provide the content for appbar and navigation.
    /// </summary>
    public interface IAppBarContentProvider
    {
        /// <summary>
        /// The action raised when the state of the service changes.
        /// </summary>
        event Action StateHasChanged;

        /// <summary>
        /// Actions.
        /// </summary>
        IReadOnlyCollection<ActionConfig> Actions { get; }

        /// <summary>
        /// Get or set the page title.
        /// </summary>
        string PageTitle { get; set; }

        /// <summary>
        /// Sets the new set of actions to show in app bar.
        /// </summary>
        /// <param name="actions"></param>
        void SetPageActions(IEnumerable<ActionConfig> actions);
    }

    /// <summary>
    /// Implementation of <see cref="IAppBarContentProvider"/>.
    /// </summary>
    public class AppBarContentProvider : IAppBarContentProvider
    {
        private List<ActionConfig> _actions = new List<ActionConfig>();
        private string _pageTitle;

        /// <inheritdoc/>
        public event Action StateHasChanged;

        /// <inheritdoc/>
        public IReadOnlyCollection<ActionConfig> Actions => _actions;

        /// <inheritdoc/>
        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                if (_pageTitle != value)
                {
                    _pageTitle = value;

                    StateHasChanged?.Invoke();
                }
            }
        }

        /// <inheritdoc/>
        public void SetPageActions(IEnumerable<ActionConfig> actions)
        {
            _actions.Clear();
            _actions.AddRange(actions);

            StateHasChanged?.Invoke();
        }
    }
}
