using ProyectoTecWeb.Models.DTOS;
using ProyectoTecWeb.Models;


    public interface IPlaylistService
    {
        Task<IEnumerable<Playlist>> GetAll();
        Task<Playlist> GetOne(Guid id);
        Task<Playlist> CreatePlaylist(CreatePlaylistDto dto, Guid userId);
        Task<Playlist> UpdatePlaylist(UpdatePlaylistDto dto, Guid id);
        Task DeletePlaylist(Guid id);
        Task AddSongToPlaylist(Guid playlistId, IEnumerable<Guid> songIds);
        Task RemoveSongFromPlaylist(Guid playlistId, Guid songId);
    }

