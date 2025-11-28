using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Repositories
{
    public interface IPlaylistRepository
    {
        Task<IEnumerable<Playlist>> GetAll();
        Task<Playlist?> GetOneWithSongs(Guid id);
        Task Add(Playlist playlist);
        Task Update(Playlist playlist);
        Task Delete(Playlist playlist);
        Task AddSongToPlaylist(PlaylistSong playlistSong);
        Task RemoveSongFromPlaylist(Guid playlistId, Guid songId);
    }
}