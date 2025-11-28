using System.Text.Json.Serialization;

namespace ProyectoTecWeb.Models
{
    public class PlaylistSong
    {
        public Guid PlaylistId { get; set; }
        [JsonIgnore]
        public Playlist Playlist { get; set; } = null!;

        public Guid SongId { get; set; }
        public Song Song { get; set; } = null!;

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
