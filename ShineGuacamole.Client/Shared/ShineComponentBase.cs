using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;

namespace ShineGuacamole.Client.Shared
{
    /// <summary>
    /// The base component.
    /// </summary>
    public class ShineComponentBase<T> : ComponentBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        [Inject]
        protected ILogger<T> Logger { get; set; }

        /// <summary>
        /// The snackbar service.
        /// </summary>
        [Inject]
        protected ISnackbar Snackbar { get; set; }

        /// <summary>
        /// The authentication state.
        /// </summary>
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationState { get; set; }

        /// <summary>
        /// The current user.
        /// </summary>
        protected ClaimsPrincipal CurrentUser { get; set; }

        /// <summary>
        /// The current user identifier.
        /// </summary>
        protected string UserId => CurrentUser?.GetUserId();

        /// <summary>
        /// The current user name.
        /// </summary>
        protected string UserName => CurrentUser?.GetUserName();
        
        /// <summary>
        /// Whether the auth state is loaded.
        /// </summary>
        protected bool IsAuthLoaded { get; private set; }

        /// <inheritdoc/>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await base.OnInitializedAsync();

                var state = await AuthenticationState;

                CurrentUser = state.User;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to initialize. {ex}");
            }
            finally
            {
                IsAuthLoaded = true;
            }
        }

        /// <summary>
        /// Notify the message and log the error with exception.
        /// </summary>
        /// <param name="message">The message to show to user.</param>
        /// <param name="ex">The exception.</param>
        protected virtual void NotifyAndLogError(string message, Exception ex)
        {
            Logger.LogError($"{message} {ex}");

            Snackbar.Add(message, Severity.Error);
        }
    }
}
