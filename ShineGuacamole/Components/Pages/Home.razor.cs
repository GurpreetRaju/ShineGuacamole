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
using MudBlazor;
using ShineGuacamole.Components.Dialogs;
using ShineGuacamole.Services.Interfaces;
using ShineGuacamole.Shared.Models;

namespace ShineGuacamole.Components.Pages
{
    /// <summary>
    /// Home.
    /// </summary>
    public partial class Home
    {
        #region Fields

        private List<RemoteConnectionInfo> _favoriteConnections = new List<RemoteConnectionInfo>();

        #endregion


        #region Properties

        /// <summary>
        /// Reference to connection service.
        /// </summary>
        [Inject]
        private IConnectionManagerService ConnectionService { get; set; }

        /// <summary>
        /// Reference to dialog service.
        /// </summary>
        [Inject]
        private IDialogService DialogService { get; set; }

        #endregion


        #region Methods

        /// <inheritdoc/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await RefreshData();
        }

        /// <summary>
        /// Refresh the data.
        /// </summary>
        /// <returns></returns>
        private async Task RefreshData()
        {
            _favoriteConnections.Clear();

            var favConns = await ConnectionService.GetFavorites(UserId);
            if (favConns?.Any() == true)
            {
                _favoriteConnections.AddRange(favConns);
            }
        }

        /// <summary>
        /// Edit favorites.
        /// </summary>
        private async Task EditFavorite()
        {
            try
            {
                var instance = await DialogService.ShowAsync<EditFavoriteConnectionsDialog>(null, new DialogOptions 
                {
                    MaxWidth = MaxWidth.Large
                });

                var result = await instance.Result;
                if (result?.Canceled == false)
                {
                    await RefreshData();
                }
            }
            catch (Exception ex)
            {
                NotifyAndLogError("Failed to open favorite connections dialog.", ex);
            }
        }

        #endregion
    }
}
