using Microsoft.EntityFrameworkCore;
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

        public async Task Delete(Song song)
        {
            _db.Songs.Remove(song);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Song>> GetAll()
        {
             return await _db.Songs.ToListAsync();
        }

        public async Task<Song?> GetOne(Guid id)
        {
            return await _db.Songs.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Song song)
        {
            _db.Songs.Update(song);
            await _db.SaveChangesAsync();
        }
    }
}
