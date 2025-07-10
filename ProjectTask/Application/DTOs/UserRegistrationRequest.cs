using System.ComponentModel.DataAnnotations;

namespace ProjectTask.Application.DTOs
{
    public class UserRegistrationRequest
    {
        [Required]
        [MinLength(4)]
        public string? Username { get; set; }

        [Required]
        [MinLength(6)]
        public string? Password { get; set; }
    }
}
