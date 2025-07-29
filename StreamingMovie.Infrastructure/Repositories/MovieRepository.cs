using System.Linq;
using Microsoft.EntityFrameworkCore;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    /// <summary>
    /// Movie repository
    /// </summary>
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieDbContext context)
            : base(context) { }

        public Task<int> GetTotalMovieAsync()
        {
            return _dbSet.CountAsync();
        }
    }
}