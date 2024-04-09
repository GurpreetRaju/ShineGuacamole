using System.ComponentModel.DataAnnotations;

namespace ShineGuacamole.Models
{
    public sealed class Connection
    {
        public Arguments Arguments { get; set; } = new();

        [Required]
        public string Type { get; set; } = default!;
    }
}