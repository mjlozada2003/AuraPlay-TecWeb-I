using System.ComponentModel.DataAnnotations;

namespace ProyectoTecWeb.Models.DTOS
{
    public class RegisterDto
    {
        [Required, StringLength(100)]
        public string Username { get; init; }

        [Required, EmailAddress]
        public string Email { get; init; }
        [Required]
        public string Password { get; init; }
        public string Role { get; set; } = "User";
    }
}
