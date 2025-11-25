using System.ComponentModel.DataAnnotations;

namespace ProyectoTecWeb.Models.DTOS
{
    public class UpdateSongDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public float duration { get; set; }

    }
}
