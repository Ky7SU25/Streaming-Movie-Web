using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Domain.Interfaces
{
    /// <summary>
    /// Interface for UserRepository
    /// </summary>
    public interface IUserRepository : IGenericRepository<User> 
    {
        Task<IEnumerable<User>> GetPremiumUsersAsync();
        Task<int> GetTotalUsersCountAsync();
    }
}