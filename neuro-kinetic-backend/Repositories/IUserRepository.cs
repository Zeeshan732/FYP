using neuro_kinetic_backend.Models;

namespace neuro_kinetic_backend.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
        Task<bool> EmailExistsAsync(string email);
    }
}

