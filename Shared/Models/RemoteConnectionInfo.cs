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
    /// The Remote Connection Info.
    /// </summary>
    public class RemoteConnectionInfo
    {
        /// <summary>
        /// The connection Identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The connection name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The connection type.
        /// </summary>
        public ConnectionType Type { get; set; }

        /// <summary>
        /// The favorite position.
        /// </summary>
        public int? FavPosition { get; set; }

        /// <summary>
        /// The image.
        /// </summary>
        public byte[] Image {  get; set; }

        /// <summary>
        /// The connection owner.
        /// </summary>
        public string Owner { get; set; }
    }

    /// <summary>
    /// The connection type.
    /// </summary>
    public enum ConnectionType
    {
        RDP
    }
}
