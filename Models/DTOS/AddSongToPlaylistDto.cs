using System.ComponentModel.DataAnnotations;

namespace ProyectoTecWeb.Models.DTOS
{
    public class AddSongToPlaylistDto
    {
        [Required]
        public List<Guid> Songs { get; set; } = new();
    }
}
