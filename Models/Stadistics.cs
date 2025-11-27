namespace ProyectoTecWeb.Models
{
    public class Stadistics
    {
        public Guid Id { get; set; }
        public int Reproductions { get; set; }
        public int Likes { get; set; }
        public int Downloads { get; set; }
        public double Rating { get; set; }

        // Clave foránea y navegación
        public Guid SongId { get; set; }
        public Song Song { get; set; }
        
    }


}

