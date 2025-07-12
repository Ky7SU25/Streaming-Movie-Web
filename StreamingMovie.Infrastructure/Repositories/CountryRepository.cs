using Microsoft.EntityFrameworkCore;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// Country repository
    /// </summary>
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        public CountryRepository(MovieDbContext context)
            : base(context) { }

        public virtual async Task<IEnumerable<int>> GetCountryIdsByCodesAsync(IEnumerable<string> codes)
        {
            return await _dbSet
                .Where(c => codes.Contains(c.Code))
                .Select(c => c.Id) 
                .ToListAsync();
        }
    }
}