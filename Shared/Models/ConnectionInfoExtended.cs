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

namespace ShineGuacamole.Shared.Models
{
    /// <summary>
    /// Stores the connection info and configuration.
    /// </summary>
    public class ConnectionInfoExtended
    {
        /// <summary>
        /// The connection info.
        /// </summary>
        public ConnectionInfo ConnectionInfo { get; set; }

        /// <summary>
        /// The connection configuration.
        /// </summary>
        public Dictionary<string, string> Configuration { get; set; }
    }
}
