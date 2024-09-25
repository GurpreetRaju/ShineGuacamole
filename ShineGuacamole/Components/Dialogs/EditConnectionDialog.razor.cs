using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using ShineGuacamole.Services.Interfaces;
using ShineGuacamole.Shared.Models;
using System.IO;
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
        /// Upload the selected image.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task UploadImage(IBrowserFile file)
        {
            try
            {
                if (file == null) return;

                await using MemoryStream stream = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(stream);

                _connection.Image = stream.ToArray();
            }
            catch (Exception ex)
            {
                NotifyAndLogError($"Failed to load the image {file.Name}.", ex);
            }
        }

        /// <summary>
        /// Remove the image.
        /// </summary>
        private void RemoveImage()
        {
            _connection.Image = null;
        }

        /// <summary>
        /// Add new connection.
        /// </summary>
        /// <returns></returns>
        private async Task SaveConnection()
        { 
            try
            {
                await ConnectionService.SaveConnection(UserId, _connection, _properties);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to get connection details for {ConnectionId}. {ex}");

                Snackbar.Add("Failed to get the connection details.");
            }
        }
    }
}
