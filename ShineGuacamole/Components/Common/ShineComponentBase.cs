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

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using ShineGuacamole.Shared;
using System.Security.Claims;

namespace ShineGuacamole.Components.Common
{
    /// <summary>
    /// The base component.
    /// </summary>
    public class ShineComponentBase<T> : ComponentBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        [Inject]
        protected ILogger<T> Logger { get; set; }

        /// <summary>
        /// The snackbar service.
        /// </summary>
        [Inject]
        protected ISnackbar Snackbar { get; set; }

        /// <summary>
        /// The current user.
        /// </summary>
        [CascadingParameter]
        public ClaimsPrincipal CurrentUser { get; set; }

        /// <summary>
        /// The current user identifier.
        /// </summary>
        protected string UserId => CurrentUser?.GetUserId();

        /// <summary>
        /// The current user name.
        /// </summary>
        protected string UserName => CurrentUser?.GetUserName();

        /// <summary>
        /// Notify the message and log the error with exception.
        /// </summary>
        /// <param name="message">The message to show to user.</param>
        /// <param name="ex">The exception.</param>
        protected virtual void NotifyAndLogError(string message, Exception ex)
        {
            Logger.LogError($"{message} {ex}");

            Snackbar.Add(message, Severity.Error);
        }
    }
}
