using ProyectoTecWeb.Models.DTOS;
using ProyectoTecWeb.Models;

public interface IPlaylistService
{
    Task<Playlist> CreatePlaylist(CreatePlaylistDto dto, Guid userId);
    Task DeletePlaylist(Guid id);
    Task<IEnumerable<Playlist>> GetAll();
    Task<Playlist> GetOne(Guid id);
    Task<Playlist> UpdatePlaylist(UpdatePlaylistDto dto, Guid id);
    Task AddSongToPlaylist(Guid playlistId, AddSongToPlaylistDto dto);
    Task RemoveSongFromPlaylist(Guid playlistId, Guid songId);
}
    public interface IPlaylistService
    {
        Task<IEnumerable<Playlist>> GetAll();
        Task<Playlist> GetOne(Guid id);
        Task<Playlist> CreatePlaylist(CreatePlaylistDto dto, Guid userId);
        Task<Playlist> UpdatePlaylist(UpdatePlaylistDto dto, Guid id);
        Task DeletePlaylist(Guid id);
        Task AddSongToPlaylist(Guid playlistId, AddSongToPlaylistDto dto);
        Task RemoveSongFromPlaylist(Guid playlistId, Guid songId);
    }
}
