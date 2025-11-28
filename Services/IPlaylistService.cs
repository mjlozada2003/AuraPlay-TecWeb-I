using ProyectoTecWeb.Models.DTOS;
using ProyectoTecWeb.Models;

public interface IPlaylistService
{
    Task<Playlist> CreatePlaylist(CreatePlaylistDto dto, Guid userId);
    Task DeletePlaylist(Guid id);
    Task<IEnumerable<Playlist>> GetAll();
    Task<Playlist> GetOne(Guid id);
    Task<Playlist> UpdatePlaylist(UpdatePlaylistDto dto, Guid id);
    Task AddSongsToPlaylist(Guid playlistId, IEnumerable<Guid> songIds);
    Task RemoveSongFromPlaylist(Guid playlistId, Guid songId);
}

