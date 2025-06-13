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
    // Core entities
    public DbSet<Country> Countries { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<VideoQuality> VideoQualities { get; set; }
    public DbSet<VideoServer> VideoServers { get; set; }

    // People entities
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Director> Directors { get; set; }

    // Content entities
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<Episode> Episodes { get; set; }

    // Video entities
    public DbSet<MovieVideo> MovieVideos { get; set; }
    public DbSet<EpisodeVideo> EpisodeVideos { get; set; }

    // Relationship entities
    public DbSet<MovieCategory> MovieCategories { get; set; }
    public DbSet<MovieActor> MovieActors { get; set; }
    public DbSet<MovieDirector> MovieDirectors { get; set; }
    public DbSet<SeriesCategory> SeriesCategories { get; set; }
    public DbSet<SeriesActor> SeriesActors { get; set; }
    public DbSet<SeriesDirector> SeriesDirectors { get; set; }

    // User interaction entities
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<PlaylistItem> PlaylistItems { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<WatchHistory> WatchHistories { get; set; }

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
