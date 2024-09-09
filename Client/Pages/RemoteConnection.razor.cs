using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using ShineGuacamole.Client.Components;
using ShineGuacamole.Client.General;

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
        private bool _connected;
        private bool _initialized;

        /// <summary>
        /// JS Runtime.
        /// </summary>
        [Inject]
        protected IJSRuntime JS { get; set; }

        /// <summary>
        /// The snackbar.
        /// </summary>
        [Inject]
        protected ISnackbar Snackbar { get; set; }

        /// <summary>
        /// The connection identifier.
        /// </summary>
        [SupplyParameterFromQuery]
        protected string ConnectionId { get; set; }

        /// <summary>
        /// App bar content provider.
        /// </summary>
        [CascadingParameter]
        public AppBarContentProvider AppBarContentProvider { get; set; }

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            AppBarContentProvider.SetPageTitle("Remote Connection");
            AppBarContentProvider.SetActions([
                 new ActionConfig { Icon = Icons.Material.Filled.Link, OnClick = () => _ = Connect(), Text = "Connect" },
                 new ActionConfig { Icon = Icons.Material.Filled.LinkOff, OnClick = () => _ = Disconnect(), Text = "Disconnect" }
            ]);
        }

        /// <summary>
        /// Called when the component is initialized.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await base.OnInitializedAsync();

                _module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/rdp-connection.js");

                _initialized = true;
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to initialized. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Connect.
        /// </summary>
        /// <returns></returns>
        protected async Task Connect()
        {
            await _module.InvokeVoidAsync("Connect", _id.ToString(), _display, ConnectionId);
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
