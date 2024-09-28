using System.Security.Claims;

namespace ShineGuacamole.Shared
{
    /// <summary>
    /// User info.
    /// </summary>
    public class UserInfo
    {
        private const string NameClaim = "name";

        public string UserId { get; set; }
        public string Name { get; set; }

        public static UserInfo FromClaimsPrincipal(ClaimsPrincipal principal) =>
            new()
            {
                UserId = GetRequiredClaim(principal, ClaimTypes.NameIdentifier),
                Name = GetRequiredClaim(principal, NameClaim),
            };

        private static string GetRequiredClaim(ClaimsPrincipal principal, string claimType) =>
            principal.FindFirst(claimType)?.Value ?? throw new InvalidOperationException($"Could not find required '{claimType}' claim.");
    }
}
