using Microsoft.EntityFrameworkCore;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Domain.Interfaces
{
    /// <summary>
    /// Interface for CountryRepository
    /// </summary>
    public interface ICountryRepository : IGenericRepository<Country> 
    {
       Task<IEnumerable<int>> GetCountryIdsByCodesAsync(IEnumerable<string> codes);
    }
}