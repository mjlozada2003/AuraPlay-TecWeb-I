using System.Text.Json.Serialization;

namespace ProyectoTecWeb.Models
{
    public class Statistics
    {
        public Guid Id { get; set; }
        public int Reproductions { get; set; }
        public int Likes { get; set; }
        public int Downloads { get; set; }
        public double Rating { get; set; }

        // Clave foránea y navegación
        public Guid SongId { get; set; }
        [JsonIgnore]
        public Song Song { get; set; } = null!;
        
    }


}

