using ProyectoTecWeb.Models.DTOS;
using ProyectoTecWeb.Models;
using ProyectoTecWeb.Repositories;

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
    public async Task AddSongsToPlaylist(Guid playlistId, IEnumerable<Guid> songIds)
    {
        var playlist = await _repo.GetOne(playlistId);
        if (playlist == null)
            throw new Exception("Playlist not found");

        foreach (var songId in songIds)
        {
            await _repo.AddSongToPlaylist(playlistId, songId);
        }
    }


    public async Task RemoveSongFromPlaylist(Guid playlistId, Guid songId)
    {
        await _repo.RemoveSongFromPlaylist(playlistId, songId);
    }
}
