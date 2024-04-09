
using Microsoft.AspNetCore.Components;
using MudBlazor;
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
        /// Remote connections state.
        /// </summary>
        [Inject]
        protected RemoteConnectionState State { get; set; }

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
        /// Called when this component is initialized.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            try
            {
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

            State.PendingConnectionId = connectionInfo.Id;
            Navigation.NavigateTo(AppRoutes.RemoteConnection);
        }
    }
}
