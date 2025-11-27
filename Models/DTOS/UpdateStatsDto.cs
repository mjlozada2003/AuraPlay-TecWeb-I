namespace ProyectoTecWeb.Models.DTOS
{
    public record UpdateStatsDto
    {
        public int Likes { get; set; }
        public int Reproductions { get; set; }
        public double Rating { get; set; }
    }
}
