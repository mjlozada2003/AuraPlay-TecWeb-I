using ProyectoTecWeb.Models;
using ProyectoTecWeb.Models.DTOS;

namespace ProyectoTecWeb.Services
{
    public interface ISongService
    {
        Task<IEnumerable<Song>> GetAll();
        Task<Song?> GetOne(Guid id);
        Task<Song> CreateSong(CreateSongDto dto);
        Task<Song> UpdateSong(UpdateSongDto dto, Guid id);
        Task DeleteSong(Guid id);
        Task<Song> UpdateStats(Guid songId, UpdateStatsDto dto);
    }
}
