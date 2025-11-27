using System.ComponentModel.DataAnnotations;

namespace ProyectoTecWeb.Models.DTOS
{
    public class AddSongToPlaylistDto
    {
        [Required]
        public Guid SongId { get; set; }
    }
}
