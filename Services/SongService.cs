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
                duration = dto.duration
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

        public async Task<Song> GetOne(Guid id)
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
    }
}
