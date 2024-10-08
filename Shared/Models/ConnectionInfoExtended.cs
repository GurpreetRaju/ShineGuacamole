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

using Newtonsoft.Json;

namespace ShineGuacamole.Shared.Models
{
    /// <summary>
    /// The interface for connection properties.
    /// </summary>
    public interface IConnectionProperties
    {
        /// <summary>
        /// The hostname.
        /// </summary>
        string Hostname { get; }
    }

    /// <summary>
    /// Stores the connection info and configuration.
    /// </summary>
    public class ConnectionProperties : IConnectionProperties
    {
        /// <summary>
        /// The Hostname.
        /// </summary>
        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        /// <summary>
        /// The Hostname.
        /// </summary>
        [JsonProperty("port")]
        public int Port { get; set; }

        /// <summary>
        /// The username.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// The password.
        /// </summary>
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

        /// <summary>
        /// The remote app.
        /// </summary>
        [JsonProperty("remote-app", NullValueHandling = NullValueHandling.Ignore)]

        public string RemoteApp { get; set; }
    }
}
