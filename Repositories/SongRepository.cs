using ProyectoTecWeb.Data;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly AppDbContext _db;
        public SongRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task Add(Song song)
        {
            await _db.Songs.AddAsync(song);
            await _db.SaveChangesAsync();
        }

        public Task Delete(Song song)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Song>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Song> GetOne(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Song song)
        {
            throw new NotImplementedException();
        }
    }
}
