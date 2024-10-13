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

namespace ShineGuacamole.Library.Services
{
    /// <summary>
    /// Implementation of <see cref="INotificationService"/>
    /// </summary>
    public class NotificationService : INotificationService
    {
        private Dictionary<NotificationType, Subscription> _subscriptions = new Dictionary<NotificationType, Subscription>();
 
        /// <inheritdoc/>
        public void Register(NotificationType notificationType, Action<INotificationItem> updateHandler)
        {
            if (!_subscriptions.ContainsKey(notificationType)) 
            {
                _subscriptions.Add(notificationType, new Subscription());
            }

            _subscriptions[notificationType].UpdateAvailable += updateHandler;
        }

        /// <inheritdoc/>
        public void SendNotification(INotificationItem notification)
        {
            if (_subscriptions.TryGetValue(notification.Type, out var subscription))
            {
                subscription.Notify(notification);
            }
        }

        /// <inheritdoc/>
        public void Unregister(NotificationType notificationType, Action<INotificationItem> updateHandler)
        {
            if (_subscriptions.TryGetValue(notificationType, out var subscription))
            {
                subscription.UpdateAvailable -= updateHandler;
            }
        }
    }

    /// <summary>
    /// Handles the subscription.
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// Event for when a notification is available.
        /// </summary>
        public event Action<INotificationItem> UpdateAvailable;

        /// <summary>
        /// Raises the update available event.
        /// </summary>
        /// <param name="notification"></param>
        public void Notify(INotificationItem notification) 
        {
            UpdateAvailable?.Invoke(notification);
        }
    }
}
