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
        public virtual async Task<IEnumerable<string>> GetNamesByMovieIdAsync(int movieId)
        {
            return await _dbSet
                .Where(sc => sc.MovieId == movieId)
                .Include(sc => sc.Category)
                .Select(sc => sc.Category.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryViewCountDto>> GetViewsByMovieCategoryAsync()
        {
            var result = await _dbSet
            .Include(mc => mc.Movie)
            .Include(mc => mc.Category)
            .GroupBy(mc => mc.Category.Name)
            .Select(g => new CategoryViewCountDto
            {
                CategoryName = g.Key,
                TotalViews = g.Sum(mc => mc.Movie.ViewCount ?? 0)
            })
            .ToListAsync();

            return result;
        }
    }
}