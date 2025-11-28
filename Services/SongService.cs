using ProyectoTecWeb.Models;
using ProyectoTecWeb.Models.DTOS;
using ProyectoTecWeb.Repositories;

namespace ProyectoTecWeb.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _repo;
        private readonly ILogger<SongService> _logger;
        public SongService(ISongRepository repo, ILogger<SongService> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        public async Task<Song> CreateSong(CreateSongDto dto)
        {
            _logger.LogInformation("🎤 Creando nueva canción: {Name}", dto.Name);
            var song = new Song
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                duration = dto.duration,
                // Inicializamos estadísticas en 0 automáticamente
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

        public async Task<Song> UpdateStats(Guid songId, UpdateStatsDto dto)
        {
            var song = await _repo.GetOne(songId);
            if (song == null)
            {
                _logger.LogError("❌ Error al actualizar stats: Canción {Id} no existe", songId);
                throw new Exception("Song not found");
            }
            // Como incluimos stats en el repo, podemos editarlas directamente
            song.Statistics.Likes = dto.Likes;
            song.Statistics.Reproductions = dto.Reproductions;
            song.Statistics.Rating = dto.Rating;

            await _repo.Update(song);
            _logger.LogInformation("✅ Stats actualizadas para canción {Id}", songId);
            return song;
        }
    }
}
