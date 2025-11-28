using System.ComponentModel.DataAnnotations;

namespace ProyectoTecWeb.Models.DTOS
{
    public class LoginDto
    {

        [Required, EmailAddress]
        public string Email { get; init; }

        [Required]
        public string Password { get; init; }

    }
}
