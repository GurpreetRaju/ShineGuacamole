using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShineGuacamole.Services.Interfaces;
using ShineGuacamole.Shared.Models;
using ConnectionInfo = ShineGuacamole.Shared.Models.ConnectionInfo;

namespace ShineGuacamole.Components.Dialogs
{
    /// <summary>
    /// Add connection.
    /// </summary>
    public partial class EditConnectionDialog
    {
        private MudForm form;
        private ConnectionInfo _connection;
        private ConnectionProperties _properties;
        private bool _isLoading = true;

        /// <summary>
        /// The connection service.
        /// </summary>
        [Inject]
        private IConnectionManagerService ConnectionService { get; set; }

        /// <summary>
        /// The snackbar.
        /// </summary>
        [Inject]
        private ISnackbar Snackbar { get; set; }

        /// <summary>
        /// The logger.
        /// </summary>
        [Inject]
        private ILogger<EditConnectionDialog> Logger { get; set; }

        /// <summary>
        /// The connection id.
        /// </summary>
        [Parameter]
        public string ConnectionId { get; set; }

        /// <inheritdoc/>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await base.OnInitializedAsync();

                if (string.IsNullOrEmpty(ConnectionId))
                {
                    _connection = new ConnectionInfo();
                    _properties = new ConnectionProperties();
                }
                else
                {
                    var result = await ConnectionService.GetConnection(ConnectionId);

                    _connection = result.Connection;
                    _properties = result.Properties as ConnectionProperties;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to get connection details for {ConnectionId}. {ex}");

                Snackbar.Add("Failed to get the connection details.");
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Add new connection.
        /// </summary>
        /// <returns></returns>
        private async Task SaveConnection()
        { 
            try
            {
                await ConnectionService.SaveConnection(string.Empty, _connection, _properties);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to get connection details for {ConnectionId}. {ex}");

                Snackbar.Add("Failed to get the connection details.");
            }
        }
    }
}
