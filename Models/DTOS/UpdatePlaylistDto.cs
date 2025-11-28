using System.ComponentModel.DataAnnotations;

namespace ProyectoTecWeb.Models.DTOS
{
    public class UpdatePlaylistDto
    {
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}
