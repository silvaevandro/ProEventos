using ProEventos.Domain.Identity;

namespace ProEventos.Infra.Data
{
    public interface IUserRepository : IGeralRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUsersByIdAsync(int id);
        Task<User> GetUsersByNameAsync(string username);
    }
}