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
using RemoteConnectionInfo = ShineGuacamole.Shared.Models.RemoteConnectionInfo;

namespace ShineGuacamole.Components.Pages
{
    /// <summary>
    /// Code behind for <see cref="Connections"/>.
    /// </summary>
    public partial class Connections : IDisposable
    {
        #region Fields

        private List<RemoteConnectionInfo> _connections;
        private bool _isLoading = true;
        
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
                _connections = connections?.ToList() ?? new List<RemoteConnectionInfo>();

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
                    }, Extensions.DialogOptions);

                await instance.Result;
            }
            catch (Exception ex)
            {
                NotifyAndLogError("Failed to create new connection.", ex);
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
                if (notification is EntityUpdate<RemoteConnectionInfo> update)
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
