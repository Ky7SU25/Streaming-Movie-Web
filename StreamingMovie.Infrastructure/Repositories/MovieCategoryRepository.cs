using Microsoft.EntityFrameworkCore;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// MovieCategory repository
    /// </summary>
    public class MovieCategoryRepository : GenericRepository<MovieCategory>, IMovieCategoryRepository
    {
        public MovieCategoryRepository(MovieDbContext context)
            : base(context) { }

        public virtual async Task<IEnumerable<int>> GetMovieIdsByCategoryIdAsync(int categoryId)
        {
            return await _dbSet
                .Where(mc => mc.CategoryId == categoryId)
                .Select(mc => mc.MovieId)
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<int>> GetMovieIdsByCategoryIdsAsync(IEnumerable<int> categoryIds)
        {
            return await _dbSet
                .Where(mc => categoryIds.Contains(mc.CategoryId))
                .Select(mc => mc.MovieId)
                .Distinct()
                .ToListAsync();
        }
    }
}