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

using System.Security.Claims;

namespace ShineGuacamole.Shared
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

        /// <summary>
        /// Get the user initials.
        /// </summary>
        /// <param name="principal">The claims principal.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetInitials(this ClaimsPrincipal principal)
        {
            var name = GetUserName(principal);
            var nameSplit = name.Split(null);

            if (nameSplit.Length == 0)
                return name;

            string initials = string.Empty;
            foreach (var split in nameSplit)
            {
                initials += split.ToUpper().FirstOrDefault();
            }

            return initials;
        }

        /// <summary>
        /// Convert to base64 image string.
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns></returns>
        public static string ToBase64Image(this byte[] imageBytes)
        {
            if (imageBytes != null)
            {
                var bytesString = Convert.ToBase64String(imageBytes);

                return $"data:image/png;base64,{bytesString}";
            }
            return null;
        }
    }
}
