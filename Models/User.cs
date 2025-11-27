using System.ComponentModel.DataAnnotations;

namespace ProyectoTecWeb.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required, StringLength(128)]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "User";//"User" | "Admin"
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public DateTime? RefreshTokenRevokedAt { get; set; }
        public string? CurrentJwtId { get; set; }
    }
}
