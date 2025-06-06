using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.Data;

/// <summary>
/// Represents the database context.
/// </summary>
public class MovieDbContext : IdentityDbContext<User, Role, int>
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(MovieDbContext).Assembly);

        RemoveAspNetPrefixInIdentityTable(builder: builder);
    }

    private static void RemoveAspNetPrefixInIdentityTable(ModelBuilder builder)
    {
        const string AspNetPrefix = "AspNet";

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();

            if (tableName.StartsWith(value: AspNetPrefix))
            {
                entityType.SetTableName(name: tableName[6..]);
            }
        }
    }
}
