using Microsoft.AspNetCore.Components.Routing;

namespace ShineGuacamole.Client.General
{
    /// <summary>
    /// Action configuration.
    /// </summary>
    public class ActionConfig
    {
        /// <summary>
        /// The href link for action.
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// The onlick action to invoke.
        /// </summary>
        public Action OnClick { get; set; }

        /// <summary>
        /// The action text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The action icon.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// The url match if <see cref="Href"/> is provided.
        /// </summary>
        public NavLinkMatch Match { get; set; }
    }
}
