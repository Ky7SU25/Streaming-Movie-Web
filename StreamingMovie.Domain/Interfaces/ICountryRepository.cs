using Microsoft.EntityFrameworkCore;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Domain.Interfaces
{
    /// <summary>
    /// Interface for CountryRepository
    /// </summary>
    public interface ICountryRepository : IGenericRepository<Country> 
    {
       Task<IEnumerable<int>> GetIdsByCodesAsync(IEnumerable<string> codes);
       Task<string> GetNameByIdAsync(int? countryId);
    }
}