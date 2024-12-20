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
using ShineGuacamole.Services.Interfaces;
using ShineGuacamole.Shared.Models;

namespace ShineGuacamole.Components.Dialogs
{
    /// <summary>
    /// Dialog to edit favorite connections.
    /// </summary>
    public partial class EditFavoriteConnectionsDialog
    {
        #region Fields

        private bool _isLoading = true;
        private RemoteConnectionInfo _selectedFavConnection;

        private List<RemoteConnectionInfo> _favConnections = new List<RemoteConnectionInfo>();
        private List<RemoteConnectionInfo> _availableConnections = new List<RemoteConnectionInfo>();

        #endregion


        #region Properties

        /// <summary>
        /// The connection service.
        /// </summary>
        [Inject]
        private IConnectionManagerService ConnectionService { get; set; }

        /// <summary>
        /// The dialog instance.
        /// </summary>
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }

        /// <summary>
        /// Can move selected connection up in favourite list.
        /// </summary>
        private bool CanMoveUp => _selectedFavConnection != null && _favConnections.IndexOf(_selectedFavConnection) is int currentIndex &&
            currentIndex > 0 && currentIndex < _favConnections.Count;

        /// <summary>
        /// Can move selected connection down in favourite list.
        /// </summary>
        private bool CanMoveDown => _selectedFavConnection != null && _favConnections.IndexOf(_selectedFavConnection) is int currentIndex &&
            currentIndex > -1 && currentIndex < _favConnections.Count - 1;

        #endregion


        #region Methods

        /// <inheritdoc/>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await base.OnInitializedAsync();

                var allConnections = await ConnectionService.GetConnections(UserId);
                if (allConnections?.Any() == true)
                {
                    _availableConnections.AddRange(allConnections);
                }

                var favConnections = await ConnectionService.GetFavorites(UserId);
                if (favConnections?.Any() == true)
                {
                    _favConnections.AddRange(favConnections);
                }
            }
            catch (Exception ex)
            {
                NotifyAndLogError("Failed to load favorite connections.", ex);
            }
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Add to favorites list.
        /// </summary>
        /// <param name="connection">The connection.</param>
        private void AddToFavorite(RemoteConnectionInfo connection)
        {
            _favConnections.Add(connection);
            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Remote from favorites list.
        /// </summary>
        /// <param name="connection">The connection.</param>
        private void RemoveFromFavorite(RemoteConnectionInfo connection)
        {
            _favConnections.Remove(connection);
            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Move up in favorites list.
        /// </summary>
        private void MoveUp()
        {
            if (!CanMoveUp) return;

            var currentIndex = _favConnections.IndexOf(_selectedFavConnection);
            _favConnections.RemoveAt(currentIndex);
            _favConnections.Insert(currentIndex - 1, _selectedFavConnection);

            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Move down in favorites list.
        /// </summary>
        /// <param name="connection">The connection.</param>
        private void MoveDown()
        {
            if (!CanMoveDown) return;

            var currentIndex = _favConnections.IndexOf(_selectedFavConnection);
            _favConnections.RemoveAt(currentIndex);
            _favConnections.Insert(currentIndex + 1, _selectedFavConnection);

            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Handle the selected connection changed.
        /// </summary>
        /// <param name="connection"></param>
        private void HandleSelectedConnectionChanged(RemoteConnectionInfo connection)
        {
            if (_selectedFavConnection == connection) return;

            _selectedFavConnection = connection;
            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        /// <returns></returns>
        private async Task SaveChanges()
        {
            try
            {
                await ConnectionService.SaveFavorites(UserId, _favConnections);

                Dialog.Close();
            }
            catch (Exception ex)
            {
                NotifyAndLogError("Failed to save favorite connections.", ex);
            }
        }

        #endregion
    }
}
