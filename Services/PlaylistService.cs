using ProyectoTecWeb.Models;
using ProyectoTecWeb.Models.DTOS;
using ProyectoTecWeb.Repositories;

namespace ProyectoTecWeb.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _repo;
        public PlaylistService(IPlaylistRepository repo)
        {
            _repo = repo;
        }

        public async Task<Playlist> CreatePlaylist(CreatePlaylistDto dto, Guid userId)
        {
            var playlist = new Playlist
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                UserId = userId
            };
            await _repo.Add(playlist);
            return playlist;
        }

        public async Task DeletePlaylist(Guid id)
        {
            var playlist = await _repo.GetOne(id);
            if (playlist == null) throw new Exception("Playlist not found.");

            await _repo.Delete(playlist);
        }

        public async Task<IEnumerable<Playlist>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<Playlist> GetOne(Guid id)
        {
            var playlist = await _repo.GetOneWithSongs(id);
            if (playlist == null) throw new Exception("Playlist not found.");
            return playlist;
        }

        public async Task<Playlist> UpdatePlaylist(UpdatePlaylistDto dto, Guid id)
        {
            var playlist = await _repo.GetOne(id);
            if (playlist == null) throw new Exception("Playlist not found.");

            if (!string.IsNullOrEmpty(dto.Name))
                playlist.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Description))
                playlist.Description = dto.Description;

            await _repo.Update(playlist);
            return playlist;
        }

        public async Task AddSongToPlaylist(Guid playlistId, AddSongToPlaylistDto dto)
        {
            var playlistSong = new PlaylistSong
            {
                PlaylistId = playlistId,
                SongId = dto.SongId,
                AddedAt = DateTime.UtcNow
            };
            await _repo.AddSongToPlaylist(playlistSong);
        }

        public async Task RemoveSongFromPlaylist(Guid playlistId, Guid songId)
        {
            await _repo.RemoveSongFromPlaylist(playlistId, songId);
        }
    }
}
