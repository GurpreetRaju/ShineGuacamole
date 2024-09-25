using System.Security.Claims;

namespace ShineGuacamole.Client
{
    /// <summary>
    /// User info.
    /// </summary>
    public sealed class UserInfo
    {
        public required string UserId { get; init; }
        public required string Name { get; init; }

        public static UserInfo FromClaimsPrincipal(ClaimsPrincipal principal) =>
            new()
            {
                UserId = GetRequiredClaim(principal, ClaimTypes.NameIdentifier),
                Name = GetRequiredClaim(principal, ClaimTypes.Name),
            };

        public ClaimsPrincipal ToClaimsPrincipal() => new (new ClaimsIdentity(
                [new(ClaimTypes.NameIdentifier, UserId), new(ClaimTypes.Name, Name)],
                authenticationType: nameof(UserInfo))
        );

        private static string GetRequiredClaim(ClaimsPrincipal principal, string claimType) =>
            principal.FindFirst(claimType)?.Value ?? throw new InvalidOperationException($"Could not find required '{claimType}' claim.");
    }
}
