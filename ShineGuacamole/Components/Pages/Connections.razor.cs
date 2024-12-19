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
using ShineGuacamole.Library.Models;
using ShineGuacamole.Library.Services.Interfaces;
using ShineGuacamole.Services.Interfaces;
using ShineGuacamole.Shared;
using ShineGuacamole.Shared.Enums;
using ConnectionInfo = ShineGuacamole.Shared.Models.ConnectionInfo;

namespace ShineGuacamole.Components.Pages
{
    /// <summary>
    /// Code behind for <see cref="Connections"/>.
    /// </summary>
    public partial class Connections : IDisposable
    {
        #region Fields

        private List<ConnectionInfo> _connections;
        private bool _isLoading = true;
        private static DialogOptions s_options = new DialogOptions
        {
            BackdropClick = false,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        #endregion


        #region Properties

        /// <summary>
        /// Reference to connection service.
        /// </summary>
        [Inject]
        private IConnectionManagerService ConnectionService { get; set; }

        /// <summary>
        /// Reference to notification service.
        /// </summary>
        [Inject]
        private INotificationService NotificationService { get; set; }

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

        #endregion


        #region Overrides and Public Methods

        /// <summary>
        /// Called when this component is initialized.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await base.OnInitializedAsync();

                var connections = await ConnectionService.GetConnections(UserId);
                _connections = connections?.ToList() ?? new List<ConnectionInfo>();

                NotificationService.Register(NotificationType.ConnectionUpdate, HandleConnectionChange);
            }
            catch (Exception ex)
            {
                NotifyAndLogError("Failed to initialize.", ex);
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            NotificationService.Unregister(NotificationType.ConnectionUpdate, HandleConnectionChange);
        }

        #endregion


        #region Private Methods

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
                NotifyAndLogError($"Failed to connect to Connection {connectionInfo?.Id}.", ex);
            }
        }

        /// <summary>
        /// Show details.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        private async Task Details(ConnectionInfo connectionInfo)
        {
            if (connectionInfo == null) return;

            await EditConnection(connectionInfo, ViewMode.ReadOnly);
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
                NotifyAndLogError("Failed to parse image data.", ex);
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
                var instance = await DialogService.ShowAsync<EditConnectionDialog>(null,                    
                    new DialogParameters<EditConnectionDialog>
                    {
                        {x => x.Mode, ViewMode.New}
                    }, s_options);

                await instance.Result;
            }
            catch (Exception ex)
            {
                NotifyAndLogError("Failed to create new connection.", ex);
            }
        }

        /// <summary>
        /// Opens the edit connection dialog.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private async Task EditConnection(ConnectionInfo connection, ViewMode mode)
        {
            try
            {
                if (connection == null) throw new ArgumentNullException(nameof(connection));

                var instance = await DialogService.ShowAsync<EditConnectionDialog>(null,
                    new DialogParameters<EditConnectionDialog>
                    {
                        { x => x.ConnectionId, connection.Id },
                        { x => x.Mode, mode },
                    }, 
                    s_options);

                await instance.Result;
            }
            catch (Exception ex)
            {
                NotifyAndLogError($"Failed to {(mode == ViewMode.ReadOnly ? "view" : "edit")} connection", ex);
            }
        }

        /// <summary>
        /// Handle the connection changes.
        /// </summary>
        /// <param name="notification">Connection change notification.</param>
        private void HandleConnectionChange(INotificationItem notification)
        {
            try
            {
                if (notification is EntityUpdate<ConnectionInfo> update)
                {
                    int index = _connections.FindIndex(c => c.Id == update.Entity.Id);
                    if (index > -1)
                    {
                        _connections[index] = update.Entity;

                        InvokeAsync(StateHasChanged);
                    }
                }
            }
            catch (Exception ex) 
            {
                NotifyAndLogError("Failed to handle the connection update.", ex);
            }
        }

        #endregion
    }
}
