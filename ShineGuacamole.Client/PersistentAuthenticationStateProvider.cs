using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Net.Http.Json;

namespace ShineGuacamole.Client
{
    /// <summary>
    /// Persistent authentication state provider.
    /// </summary>
    public class PersistentAuthenticationStateProvider : AuthenticationStateProvider
    {
        private static readonly AuthenticationState DefaultUnauthenticatedState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        private readonly Task<AuthenticationState> _authenticationStateTask;
        private readonly HttpClient _httpClient;
        private readonly ILogger<PersistentAuthenticationStateProvider> _logger;

        public PersistentAuthenticationStateProvider(HttpClient httpClient, ILogger<PersistentAuthenticationStateProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _authenticationStateTask = LoadAuthenticationState();
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() => _authenticationStateTask;

        private async Task<AuthenticationState> LoadAuthenticationState()
        {
            try
            {
                if (_authenticationStateTask == null)
                {
                    var res = await _httpClient.GetAsync("/authState");
                    if (res.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var info = await res.Content.ReadFromJsonAsync<UserInfo>();
                        return new AuthenticationState(info.ToClaimsPrincipal());
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting the auth state. {ex}");
            }
            return DefaultUnauthenticatedState;
        }
    }
}
