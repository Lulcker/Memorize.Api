using System.ComponentModel.DataAnnotations;

namespace Memorize.Library.Request
{
    public class RegistrationRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
