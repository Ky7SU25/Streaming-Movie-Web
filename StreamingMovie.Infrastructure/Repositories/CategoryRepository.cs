using Microsoft.EntityFrameworkCore;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Domain.UnitOfWorks;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// Category repository
    /// </summary>
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(MovieDbContext context)
            : base(context) { }

        public virtual async Task<IEnumerable<int>> GetCategoryIdsBySlugsAsync(IEnumerable<string> slugs)
        {
            return await _dbSet
                .Where(c => slugs.Contains(c.Slug))
                .Select(c => c.Id)
                .ToListAsync();
        }
    }
}