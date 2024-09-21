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

using Microsoft.AspNetCore.Components.Routing;

namespace ShineGuacamole.Shared.Models
{
    /// <summary>
    /// Action configuration.
    /// </summary>
    public class ActionConfig
    {
        /// <summary>
        /// The href link for action.
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// The onlick action to invoke.
        /// </summary>
        public Action OnClick { get; set; }

        /// <summary>
        /// The action text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The action icon.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// The url match if <see cref="Href"/> is provided.
        /// </summary>
        public NavLinkMatch Match { get; set; }
    }
}
