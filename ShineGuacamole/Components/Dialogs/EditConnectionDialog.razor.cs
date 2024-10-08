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
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using ShineGuacamole.Services.Interfaces;
using ShineGuacamole.Shared.Enums;
using ShineGuacamole.Shared.Models;
using ConnectionInfo = ShineGuacamole.Shared.Models.ConnectionInfo;

namespace ShineGuacamole.Components.Dialogs
{
    /// <summary>
    /// Add connection.
    /// </summary>
    public partial class EditConnectionDialog
    {
        private MudForm _form;
        private ConnectionInfo _connection;
        private ConnectionProperties _properties;
        private bool _isLoading = true;
        private bool _isValid;

        /// <summary>
        /// The connection service.
        /// </summary>
        [Inject]
        private IConnectionManagerService ConnectionService { get; set; }

        /// <summary>
        /// The connection id.
        /// </summary>
        [Parameter]
        public string ConnectionId { get; set; }

        /// <summary>
        /// View mode.
        /// </summary>
        [Parameter]
        public ViewMode Mode { get; set; } 

        /// <summary>
        /// The dialog instance.
        /// </summary>
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }

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

                Dialog.Close();
            }
            catch (Exception ex)
            {
                NotifyAndLogError($"Failed to save connection details {ConnectionId}.", ex);
            }
        }
    }
}
