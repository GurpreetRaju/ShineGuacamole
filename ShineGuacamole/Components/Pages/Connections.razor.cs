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
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using ShineGuacamole.Components.Dialogs;
using ShineGuacamole.Services.Interfaces;
using ShineGuacamole.Shared;
using ConnectionInfo = ShineGuacamole.Shared.Models.ConnectionInfo;

namespace ShineGuacamole.Components.Pages
{
    /// <summary>
    /// Code behind for <see cref="Connections"/>.
    /// </summary>
    public partial class Connections
    {
        private IEnumerable<ConnectionInfo> _connections;
        private bool _isLoading = true;
        private static DialogOptions s_options = new DialogOptions
        {
            BackdropClick = false,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        /// <summary>
        /// Reference to connection service.
        /// </summary>
        [Inject]
        private IConnectionManagerService ConnectionService { get; set; }

        /// <summary>
        /// Reference to the snackbar.
        /// </summary>
        [Inject]
        private ISnackbar Snackbar { get; set; }

        /// <summary>
        /// Reference to the Navigation Manager.
        /// </summary>
        [Inject]
        private NavigationManager Navigation { get; set; }
        
        /// <summary>
        /// Reference to the Dialog service.
        /// </summary>
        [Inject]
        private IDialogService DialogService { get; set; }

        /// <summary>
        /// Page actions.
        /// </summary>
        protected IEnumerable<ActionConfig> PageActions => [
            new ActionConfig { Icon = Icons.Material.Filled.Add, OnClick = () => _ = CreateNewConnection(), Text = "New connection" }
        ];

        /// <summary>
        /// Called when this component is initialized.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await base.OnInitializedAsync();
                _connections = await ConnectionService.GetConnections(UserId);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to initialize. {ex}");
                Snackbar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Connect.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        private void Connect(ConnectionInfo connectionInfo)
        {
            try
            {
                if (connectionInfo == null) return;

                var parameters = new Dictionary<string, string> { { CommonStrings.ConnectionIdParameter, connectionInfo.Id } };
                var url = QueryHelpers.AddQueryString(AppRoutes.RemoteConnection, parameters);
                Navigation.NavigateTo(url);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to connect. Connection {connectionInfo?.Id}. {ex}");
                Snackbar.Add("Failed to connect.");
            }
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
                var img = imageData.ToBase64Image();
                if (img != null) return img;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to parse image data. {ex}");
                Snackbar.Add("Failed to parse image data.");
            }
            return "images/rdp-image.png";
        }

        /// <summary>
        /// Opens the create new connection dialog.
        /// </summary>
        /// <returns></returns>
        private async Task CreateNewConnection()
        {
            try
            {
                var instance = await DialogService.ShowAsync<EditConnectionDialog>(null, s_options);

                await instance.Result;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to create new connection. {ex}");   
                Snackbar.Add("Failed to create new connection.");
            }
        }

        /// <summary>
        /// Opens the edit connection dialog.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private async Task EditConnection(ConnectionInfo connection)
        {
            try
            {
                if (connection == null) throw new ArgumentNullException(nameof(connection));

                var instance = await DialogService.ShowAsync<EditConnectionDialog>(null,
                    new DialogParameters<EditConnectionDialog>
                    {
                        { x => x.ConnectionId, connection.Id }
                    }, 
                    s_options);

                await instance.Result;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to edit connection. {ex}");
                Snackbar.Add("Failed to edit connection.");
            }
        }
    }
}
