#region Copyright
//
// Copyright 2022 ManuelExpunged
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

using System.ComponentModel.DataAnnotations;

namespace ShineGuacamole.Models
{
    public sealed class Connection
    {
        public Arguments Arguments { get; set; } = new();

        [Required]
        public string Type { get; set; } = default!;
    }

    public sealed class Arguments : Dictionary<string, string>
    {
        public new string this[string key]
        {
            get
            {
                TryGetValue(key, out string value);
                return value;
            }

            set
            {
                base[key] = value!;
            }
        }
    }
}