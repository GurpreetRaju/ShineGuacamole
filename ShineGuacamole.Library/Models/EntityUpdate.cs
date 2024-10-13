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

using ShineGuacamole.Library.Services.Interfaces;

namespace ShineGuacamole.Library.Models
{
    /// <summary>
    /// Represents a entity update.
    /// </summary>
    public class EntityUpdate<TEntity> : INotificationItem
    {
        /// <summary>
        /// The notification type.
        /// </summary>
        public NotificationType Type { get; set; }

        /// <summary>
        /// The user who updated the entity.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The updated entity.
        /// </summary>
        public TEntity Entity { get; set; }
    }
}
