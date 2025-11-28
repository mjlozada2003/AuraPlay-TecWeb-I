using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Repositories
{
    public interface ISongRepository
    {
        Task<IEnumerable<Song>> GetAll();
        Task<Song?> GetOne(Guid id);
        Task Add(Song song);
        Task Update(Song song);
        Task Delete(Song song);
    }
}
