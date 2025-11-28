using System.ComponentModel.DataAnnotations;

namespace ProyectoTecWeb.Models
{
    public class Song
    {
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Description { get; set; } = string.Empty;

        [Required, Range(0,60)]
        public float duration { get; set; }

        public Statistics? Statistics { get; set; }

        public Guid StatisticsId { get; set; }
        public Statistics statistics { get; set; }
    }
}
