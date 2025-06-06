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
    }
}