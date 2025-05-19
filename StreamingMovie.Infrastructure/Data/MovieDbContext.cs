
using Microsoft.EntityFrameworkCore;

namespace StreamingMovie.Infrastructure.Data;

public class MovieDbContext : DbContext
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
    {
    }
}
