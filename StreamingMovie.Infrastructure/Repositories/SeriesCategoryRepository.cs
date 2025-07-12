using Microsoft.EntityFrameworkCore;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// SeriesCategory repository
    /// </summary>
    public class SeriesCategoryRepository : GenericRepository<SeriesCategory>, ISeriesCategoryRepository
    {
        public SeriesCategoryRepository(MovieDbContext context)
            : base(context) { }
        public virtual async Task<IEnumerable<int>> GetSeriesIdsByCategoryIdAsync(int categoryId)
        {
            return await _dbSet
                .Where(sc => sc.CategoryId == categoryId)
                .Select(sc => sc.SeriesId)
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<int>> GetSeriesIdsByCategoryIdsAsync(IEnumerable<int> categoryIds)
        {
            return await _dbSet
                .Where(sc => categoryIds.Contains(sc.CategoryId))
                .Select(sc => sc.SeriesId)
                .Distinct()
                .ToListAsync();
        }
    }
}