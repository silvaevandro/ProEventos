using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Infra.Data.Context;

namespace ProEventos.Infra.Data.Repository
{
    public class UserRepository : GeralRepository, IUserRepository
    {
        private readonly ProEventosContext context;

        public UserRepository(ProEventosContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User> GetUsersByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<User> GetUsersByNameAsync(string? username)
        {
            return await context.Users.SingleOrDefaultAsync(user => user.UserName == username.ToLower());
        }
    }
}