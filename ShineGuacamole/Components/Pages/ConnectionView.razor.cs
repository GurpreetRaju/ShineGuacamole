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

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShineGuacamole.Services.Interfaces;
using ShineGuacamole.Shared.Enums;
using ShineGuacamole.Shared.Models;
using ShineGuacamole.Shared;

namespace ShineGuacamole.Components.Pages
{
    /// <summary>
    /// The connection view.
    /// </summary>
    public partial class ConnectionView
    {
        private MudForm _form;
        private RemoteConnectionInfo _connection;
        private ConnectionProperties _properties;
        private bool _isLoading = true;
        private bool _isValid;
        private ViewMode _mode = ViewMode.New;

        /// <summary>
        /// The connection service.
        /// </summary>
        [Inject]
        private IConnectionManagerService ConnectionService { get; set; }

        /// <summary>
        /// The connection id.
        /// </summary>
        [SupplyParameterFromQuery]
        public string ConnectionId { get; set; }

        /// <summary>
        /// View mode.
        /// </summary>
        [Parameter]
        public string Mode { get; set; }

        /// <summary>
        /// Whether user can view the connection.
        /// </summary>
        private bool CanView => _mode == ViewMode.New || CurrentUser?.Is(_connection?.Owner) == true;

        /// <summary>
        /// Can edit.
        /// </summary>
        private bool CanEdit => _mode != ViewMode.View;

        /// <inheritdoc/>
        protected override async Task OnParametersSetAsync()
        {
            try
            {
                await base.OnParametersSetAsync();

                if (!string.IsNullOrEmpty(Mode) && Enum.TryParse(Mode, true, out ViewMode mode))
                {
                    _mode = mode;
                }

                if (_connection?.Id != ConnectionId)
                {
                    if (_mode == ViewMode.New)
                    {
                        _connection = new RemoteConnectionInfo();
                        _properties = new ConnectionProperties();
                    }
                    else
                    {
                        await RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {
                NotifyAndLogError($"Failed to get connection details for {ConnectionId}.", ex);
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Refresh the data.
        /// </summary>
        private async Task RefreshData()
        {
            try
            {
                if (string.IsNullOrEmpty(ConnectionId)) return;

                var result = await ConnectionService.GetConnection(ConnectionId);

                _connection = result.Connection;
                _properties = result.Properties as ConnectionProperties;

                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                NotifyAndLogError("Failed to load the connection details.", ex);
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

                MemoryStream stream = new MemoryStream();
                await file.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024).CopyToAsync(stream);
                byte[] imageBytes = stream.ToArray();

                _connection.Image = imageBytes;
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
                await _form.Validate();
                if (!_form.IsValid)
                {
                    return;
                }

                await ConnectionService.SaveConnection(UserId, _connection, _properties);

                Snackbar.Add("Connection saved succesfully.");
            }
            catch (Exception ex)
            {
                NotifyAndLogError($"Failed to save connection details {ConnectionId}.", ex);
            }
        }
    }
}
