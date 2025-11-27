using ProyectoTecWeb.Models;
using ProyectoTecWeb.Models.DTOS;

namespace ProyectoTecWeb.Services
{
    public interface IPlaylistService
    {
        Task<IEnumerable<Playlist>> GetAll();
        Task<Playlist> GetOne(Guid id);
        Task<Playlist> CreatePlaylist(CreatePlaylistDto dto);
        Task<Playlist> UpdatePlaylist(UpdatePlaylistDto dto, Guid id);
        Task DeletePlaylist(Guid id);
        Task AddSongToPlaylist(Guid playlistId, AddSongToPlaylistDto dto);
        Task RemoveSongFromPlaylist(Guid playlistId, Guid songId);
    }
}