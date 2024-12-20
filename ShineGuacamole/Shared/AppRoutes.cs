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

using Microsoft.AspNetCore.WebUtilities;
using ShineGuacamole.Shared.Enums;

namespace ShineGuacamole.Shared
{
    /// <summary>
    /// Manages the app routes.
    /// </summary>
    public static class AppRoutes
    {
        /// <summary>
        /// Connections page.
        /// </summary>
        public const string Connections = "/connections";

        /// <summary>
        /// Remote Session page.
        /// </summary>
        public const string RemoteSession = "/remote-session";

        /// <summary>
        /// Connection page route.
        /// </summary>
        public const string Connection = "/connection/{Mode}";

        /// <summary>
        /// Connection page route format.
        /// </summary>
        public static string GetConnectionUrl(ViewMode mode)
        {
            return string.Format("/connection/{0}", mode.ToString().ToLowerInvariant());
        }
    }
}
