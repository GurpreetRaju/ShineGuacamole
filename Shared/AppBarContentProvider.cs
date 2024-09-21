using ShineGuacamole.Shared.Models;

namespace ShineGuacamole.Shared
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
