using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async  Task AddAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByEmailAddress(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
