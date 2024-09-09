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

using ShineGuacamole.Models;

namespace ShineGuacamole.Core
{
    internal static class ProtocolHelper
    {
        public static string?[] BuildHandshakeReply(Connection connection, string handshake)
        {
            string[] args = handshake.Split(',');

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                string argKey = arg[(arg.IndexOf('.') + 1)..];
                args[i] = connection.Arguments[argKey]!;
            }

            return args;
        }

        public static string BuildProtocol(params string?[] args)
        {
            string[] result = new string[args.Length];

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i] ?? string.Empty;
                result[i] = $"{arg.Length}.{arg}";
            }

            return string.Join(',', result) + ";";
        }

        public static (string content, int index) ReadProtocolUntilLastDelimiter(string content)
        {
            int index = content.LastIndexOf(';');

            if (index == -1)
                return (string.Empty, index);

            if (content.Length - 1 == index)
                return (content, content.Length);

            index += 1;

            return (content[..index], index);
        }
    }
}