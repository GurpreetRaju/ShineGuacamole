using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShineGuacamole.Shared.Enums;
using ShineGuacamole.Shared;
using Microsoft.AspNetCore.Components;

namespace ShineGuacamole.Components
{
    /// <summary>
    /// Navigation extensions.
    /// </summary>
    public static class NavigationExtensions
    {
        /// <summary>
        /// Go to the connection view.
        /// </summary>
        /// <returns></returns>
        public static void ToConnectionView(this NavigationManager navigation, ViewMode mode, string connectionId)
        {
            string url = AppRoutes.GetConnectionUrl(mode);
            if (!string.IsNullOrEmpty(connectionId))
            {
                var parameters = new Dictionary<string, string> { { CommonStrings.ConnectionIdParameter, connectionId } };
                url = QueryHelpers.AddQueryString(url, parameters);
            }
            navigation.NavigateTo(url, true);
        }

        /// <summary>
        /// Go to the remote session view.
        /// </summary>
        /// <returns></returns>
        public static void ToRemoteSessionView(this NavigationManager navigation, string connectionId)
        {
            var parameters = new Dictionary<string, string> { { CommonStrings.ConnectionIdParameter, connectionId } };
            var url = QueryHelpers.AddQueryString(AppRoutes.RemoteSession, parameters);
            navigation.NavigateTo(url, true);
        }
    }
}
