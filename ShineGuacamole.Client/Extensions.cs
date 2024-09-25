using System.Security.Claims;

namespace ShineGuacamole.Client
{
    /// <summary>
    /// The extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Get the user identifier.
        /// </summary>
        /// <param name="principal">The claims principal.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// Get the user name.
        /// </summary>
        /// <param name="principal">The claims principal.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetUserName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var claim = principal.FindFirst(ClaimTypes.Name) ?? principal.FindFirst("name");

            return claim?.Value;
        }
    }
}
