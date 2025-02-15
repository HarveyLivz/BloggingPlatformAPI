using System.ComponentModel.DataAnnotations;

namespace Api.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual Post? Post { get; set; } // One-To-One relation with Post
    }
}
