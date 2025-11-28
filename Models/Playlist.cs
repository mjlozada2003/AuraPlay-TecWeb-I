using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoTecWeb.Models
{
    public class Playlist
    {
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // RELACIÓN 1:N (Dueño de la playlist)
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Relación M:N con Cancion a través de la tabla intermedia
        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
    }
}