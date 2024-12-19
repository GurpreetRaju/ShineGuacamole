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

using System;
using System.Collections.Generic;

namespace ShineGuacamole.DataAccess.SqlServer.Models;

/// <summary>
/// The connection.
/// </summary>
public partial class Connection
{
    /// <summary>
    /// The connection id.
    /// </summary>
    public Guid ConnectionId { get; set; }

    /// <summary>
    /// The connection name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The connection type.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// The connection image.
    /// </summary>
    public byte[] Image { get; set; }

    /// <summary>
    /// The date time when created.
    /// </summary>
    public DateTime CreationDateTime { get; set; }

    /// <summary>
    /// The connection properties json.
    /// </summary>
    public string Properties { get; set; }

    /// <summary>
    /// The connection tags.
    /// </summary>
    public string Tags { get; set; }

    /// <summary>
    /// The user identifier.
    /// </summary>
    public string Owner { get; set; }

    /// <summary>
    /// The position in favorite list.
    /// </summary>
    public int? FavPosition { get; set; }
}