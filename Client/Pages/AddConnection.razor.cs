using Microsoft.AspNetCore.Components;
using ShineGuacamole.Client.Services;
using ShineGuacamole.Shared.Models;

namespace ShineGuacamole.Client.Pages
{
    public partial class AddConnection
    {
        /// <summary>
        /// The conncetion service.
        /// </summary>
        [Inject]
        private ConnectionService ConnectionService { get; set; }

        /// <summary>
        /// Add new connection.
        /// </summary>
        /// <returns></returns>
        private async Task SaveConnection()
        {
            var connection = new ConnectionInfoExtended
            {
                ConnectionInfo = new ConnectionInfo
                {
                    Id = "some_id",
                    Name = "username",
                    Type = "RDP",
                    Image = []
                },
                Configuration = new Dictionary<string, string>
                {
                    { "type", "RDP" },
                    { "hostname", "172.23.250.3" },
                    { "username", "testvm" },
                    { "password", "testvm" },
                    { "remote-app", "||wmplayer2" }
                }
            };

            await ConnectionService.SaveConnection(connection);
        }
    }
}
