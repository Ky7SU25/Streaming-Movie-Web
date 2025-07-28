using Microsoft.EntityFrameworkCore;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// User repository
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(MovieDbContext context)
            : base(context) { }

        public async Task<IEnumerable<User>> GetPremiumUsersAsync()
        {
            var now = DateTime.Now;
            return await _dbSet
                .Where(u => u.SubscriptionType == "Premium" &&
                            u.SubscriptionEndDate.HasValue &&
                            u.SubscriptionEndDate < now)
                .ToListAsync();

        }
    }
}