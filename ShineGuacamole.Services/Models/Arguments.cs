﻿namespace ShineGuacamole.Models
{
    public sealed class Arguments : Dictionary<string, string>
    {
        public new string? this[string key]
        {
            get
            {
                TryGetValue(key, out string? value);
                return value;
            }

            set
            {
                base[key] = value!;
            }
        }
    }
}