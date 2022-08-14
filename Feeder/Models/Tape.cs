using System.ComponentModel.DataAnnotations;

namespace Feeder.Models
{
    public class Tape
    {
        [Required]
        public string Channel { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public bool Enabled { get; set; }
    }
}
