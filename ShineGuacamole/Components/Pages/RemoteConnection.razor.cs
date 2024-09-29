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
using Microsoft.JSInterop;
using MudBlazor;
using ShineGuacamole.Shared;

namespace ShineGuacamole.Components.Pages
{
    /// <summary>
    /// Code behind for <see cref="RemoteConnection"/>.
    /// </summary>
    public partial class RemoteConnection : IAsyncDisposable
    {
        private IJSObjectReference _module;
        private Guid _id = new Guid();
        private ElementReference _display;
        private bool _connected;
        private bool _initialized;

        /// <summary>
        /// JS Runtime.
        /// </summary>
        [Inject]
        protected IJSRuntime JS { get; set; }

        /// <summary>
        /// The connection identifier.
        /// </summary>
        [SupplyParameterFromQuery]
        protected string ConnectionId { get; set; }

        /// <inheritdoc/>
        protected IEnumerable<ActionConfig> PageActions => [
            new ActionConfig { Icon = Icons.Material.Filled.Link, OnClick = () => _ = Connect(), Text = "Connect" },
            new ActionConfig { Icon = Icons.Material.Filled.LinkOff, OnClick = () => _ = Disconnect(), Text = "Disconnect" }
        ];

        /// <inheritdoc/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender);

                if (firstRender)
                {
                    _module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/rdp-connection.js");

                    _initialized = true;
                }
            }
            catch (Exception ex)
            {
                NotifyAndLogError($"Failed to initialize", ex);
            }
        }

        /// <summary>
        /// Connect.
        /// </summary>
        /// <returns></returns>
        protected async Task Connect()
        {
            try
            {
                await _module.InvokeVoidAsync("Connect", _id.ToString(), _display, ConnectionId);
                _connected = true;
            }
            catch (Exception ex)
            {
                NotifyAndLogError("Failed to connect.", ex);
            }
        }

        /// <summary>
        /// Disconnect.
        /// </summary>
        /// <returns></returns>
        protected async Task Disconnect()
        {
            try
            {
                if (_connected)
                {
                    await _module.InvokeVoidAsync("Disconnect", _id.ToString(), _display);
                }
            }
            catch (Exception ex)
            {
               NotifyAndLogError("Failed to disconnect.", ex);
            }
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <returns></returns>
        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (_module is not null)
            {
                await Disconnect();
                await _module.DisposeAsync();
            }
        }
    }
}
