using Microsoft.EntityFrameworkCore;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StreamingMovie.Infrastructure.Repositories
{
    public class UnifiedMovieRepository : IUnifiedMovieRepository
    {
        protected readonly MovieDbContext _dbContext;
        protected readonly DbSet<UnifiedMovie> _dbSet;

        public UnifiedMovieRepository(MovieDbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<UnifiedMovie>();
        }


        public virtual async Task<IEnumerable<UnifiedMovie>> GetAllAsync()
        {
            return await _dbSet.ToListAsync(); 
        }

        public virtual IQueryable<UnifiedMovie> Query(params Expression<Func<UnifiedMovie, object>>[] includes)
        {
            IQueryable<UnifiedMovie> query = _dbSet;
            if (includes != null)
            {
                foreach (var eagerEntity in includes)
                {
                    query = query.Include(eagerEntity);
                }
            }
            return query;
        }

        public virtual IQueryable<UnifiedMovie> Find(params Expression<Func<UnifiedMovie, bool>>[] predicates)
        {
            IQueryable<UnifiedMovie> query = _dbSet;
            foreach (var prep in predicates)
            {
                query = query.Where(prep);
            }
            return query;
        }
    }
}
