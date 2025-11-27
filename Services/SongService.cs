using ProyectoTecWeb.Models;
using ProyectoTecWeb.Models.DTOS;
using ProyectoTecWeb.Repositories;

namespace ProyectoTecWeb.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _repo;
        public SongService(ISongRepository repo)
        {
            _repo = repo;
        }

        public async Task<Song> CreateSong(CreateSongDto dto)
        {
            var song = new Song
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                duration = dto.duration,
                Statistics = new Statistics
                {
                    Id = Guid.NewGuid(),
                    Downloads = 0,
                    Likes = 0,
                    Rating = 0,
                    Reproductions = 0
                }
            };
            await _repo.Add(song);
            return song;
        }

        public Task DeleteSong(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Song>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<Song?> GetOne(Guid id)
        {
            return await _repo.GetOne(id);
        }

        public async Task<Song> UpdateSong(UpdateSongDto dto, Guid id)
        {
            Song? song = await GetOne(id);
            if (song == null) throw new Exception("Song doesnt exist.");

            song.Name = dto.Name;
            song.Description = dto.Description;
            song.duration = dto.duration;

            await _repo.Update(song);
            return song;
        }

        public async Task<Song> UpdateStats(Guid songId, UpdateStatsDto dto)
        {
            var song = await _repo.GetOne(songId);
            if (song == null) throw new Exception("Song not found");

            if (song.Statistics == null)
            {
                song.Statistics = new Statistics
                {
                    Id = Guid.NewGuid(),
                    Downloads = 0,
                    Likes = 0,
                    Rating = 0,
                    Reproductions = 0,
                    SongId = songId
                };
            }

            song.Statistics.Likes = dto.Likes;
            song.Statistics.Reproductions = dto.Reproductions;
            song.Statistics.Rating = dto.Rating;

            await _repo.Update(song);
            return song;
        }
    }
}