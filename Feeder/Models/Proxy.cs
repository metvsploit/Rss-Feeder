using System.ComponentModel.DataAnnotations;

namespace Feeder.Models
{
    public class Proxy
    {
        [Required]
        public string Url { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsEnabled { get; set; }
    }
}
