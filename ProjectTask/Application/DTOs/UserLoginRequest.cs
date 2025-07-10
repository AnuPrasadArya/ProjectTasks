using System.ComponentModel.DataAnnotations;

namespace ProjectTask.Application.DTOs
{
    public class UserLoginRequest
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
