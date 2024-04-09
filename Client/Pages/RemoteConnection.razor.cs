using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShineGuacamole.Client.Services;

namespace ShineGuacamole.Client.Pages
{
    /// <summary>
    /// Code behind for <see cref="RemoteConnection"/>.
    /// </summary>
    public partial class RemoteConnection : IAsyncDisposable
    {
        private IJSObjectReference _module;
        private Guid _id = new Guid();
        private ElementReference _display;
        private string _connectionId;
        private bool _connected;

        /// <summary>
        /// JS Runtime.
        /// </summary>
        [Inject]
        protected IJSRuntime JS { get; set; }

        /// <summary>
        /// Remote connection state.
        /// </summary>
        [Inject]
        protected RemoteConnectionState State { get; set; }

        /// <summary>
        /// Called when the component is initialized.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _connectionId = State.PendingConnectionId;
            State.PendingConnectionId = null;
        }

        /// <summary>
        /// Called after the component is rendered.
        /// </summary>
        /// <param name="firstRender">Whether this is first render.</param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/rdp-connection.js");
            }
        }

        /// <summary>
        /// Connect.
        /// </summary>
        /// <returns></returns>
        protected async Task Connect()
        {
            await _module.InvokeVoidAsync("Connect", _id.ToString(), _display, _connectionId);
            _connected = true;
        }

        /// <summary>
        /// Disconnect.
        /// </summary>
        /// <returns></returns>
        protected async Task Disconnect()
        {
            if (_connected)
            {
                await _module.InvokeVoidAsync("Disconnect", _id.ToString(), _display);
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
