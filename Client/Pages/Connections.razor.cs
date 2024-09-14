
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using ShineGuacamole.Client.Components;
using ShineGuacamole.Client.General;
using ShineGuacamole.Client.Services;
using ShineGuacamole.Shared.Models;

namespace ShineGuacamole.Client.Pages
{
    /// <summary>
    /// Code behind for <see cref="Connections"/>.
    /// </summary>
    public partial class Connections
    {
        private ConnectionInfo[] _connections;

        /// <summary>
        /// Reference to connection service.
        /// </summary>
        [Inject]
        protected ConnectionService ConnectionService { get; set; }

        /// <summary>
        /// Reference to the snackbar.
        /// </summary>
        [Inject]
        protected ISnackbar Snackbar { get; set; }

        /// <summary>
        /// Reference to the Navigation Manager.
        /// </summary>
        [Inject]
        protected NavigationManager Navigation { get; set; }

        /// <summary>
        /// Appbar content provider.
        /// </summary>
        [CascadingParameter]
        public AppBarContentProvider AppBarContentProvider { get; set; }

        /// <summary>
        /// Called when this component is initialized.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            try
            {
                AppBarContentProvider.SetPageTitle("Connections");
                _connections = await ConnectionService.GetConnections();
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }

        /// <summary>
        /// Connect.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        private void Connect(ConnectionInfo connectionInfo)
        {
            if (connectionInfo == null) return;

            var parameters = new Dictionary<string, string> { { CommonStrings.ConnectionIdParameter, connectionInfo.Id } };
            var url = QueryHelpers.AddQueryString(AppRoutes.RemoteConnection, parameters);
            Navigation.NavigateTo(url);
        }

        /// <summary>
        /// Gets the image source from bytes data.
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        private string GetImageSrc(byte[] imageData)
        {
            try
            {
                if (imageData != null)
                {
                    var bytesString = Convert.ToBase64String(imageData);

                    return $"data:image/png;base64,{bytesString}";
                }
            } 
            catch (Exception ex)
            {
                Snackbar.Add("Failed to parse image data.");
            }
            return "images/rdp-image.png";
        }
    }
}
