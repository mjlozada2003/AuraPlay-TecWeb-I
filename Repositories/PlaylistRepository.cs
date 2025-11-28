using Microsoft.EntityFrameworkCore;
using ProyectoTecWeb.Data;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly AppDbContext _db;
        public PlaylistRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task Add(Playlist playlist)
        {
            await _db.Playlists.AddAsync(playlist);
            await _db.SaveChangesAsync();
        }


        public async Task AddSongToPlaylist(Guid playlistId, Guid songId)
        {
            var exists = await _db.PlaylistSongs
                 .AnyAsync(ps => ps.PlaylistId == playlistSong.PlaylistId
                     && ps.SongId == playlistSong.SongId);

            if (exists) return;

            await _db.PlaylistSongs.AddAsync(playlistSong);
            await _db.SaveChangesAsync();
        }


        public async Task Delete(Playlist playlist)
        {
            _db.Playlists.Remove(playlist);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Playlist>> GetAll()
        {
            return await _db.Playlists
                .Include(p => p.PlaylistSongs)  //  Songs -> PlaylistSongs
                    .ThenInclude(ps => ps.Song) //  Incluir la canción relacionada
                .ToListAsync();
        }

        public async Task<Playlist?> GetOneWithSongs(Guid id)
        {
            return await _db.Playlists
                .Include(p => p.PlaylistSongs) 
                    .ThenInclude(ps => ps.Song) 
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task RemoveSongFromPlaylist(Guid playlistId, Guid songId)
        {
            var playlistSong = await _db.PlaylistSongs
                .FirstOrDefaultAsync(ps => ps.PlaylistId == playlistId && ps.SongId == songId);

            if (playlistSong != null)
            {
                _db.PlaylistSongs.Remove(playlistSong);
                await _db.SaveChangesAsync();
            }
        }

        public async Task Update(Playlist playlist)
        {
            _db.Playlists.Update(playlist);
            await _db.SaveChangesAsync();
        }
    }
}