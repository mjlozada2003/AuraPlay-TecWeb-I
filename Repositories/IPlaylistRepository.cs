using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Repositories
{
    public interface IPlaylistRepository
    {
        Task<IEnumerable<Playlist>> GetAll();
        Task<Playlist?> GetOne(Guid id);
        Task<Playlist?> GetOneWithSongs(Guid id);
        Task Add(Playlist playlist);
        Task Update(Playlist playlist);
        Task Delete(Playlist playlist);
        Task AddSongToPlaylist(Guid playlistId,Guid songId);
        Task RemoveSongFromPlaylist(Guid playlistId, Guid songId);
    }
}