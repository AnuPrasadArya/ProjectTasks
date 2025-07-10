using System.ComponentModel.DataAnnotations;

namespace ProjectTask.Domain.Entities
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string? Username { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

        public List<Projects> Projects { get; set; } = new();
    }
}
