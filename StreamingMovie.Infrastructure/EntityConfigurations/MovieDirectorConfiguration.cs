using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for MovieDirector
    /// </summary>
    public class MovieDirectorConfiguration : IEntityTypeConfiguration<MovieDirector>
    {
        public void Configure(EntityTypeBuilder<MovieDirector> builder)
        {
            // Table name
            builder.ToTable("MovieDirector");

            // Primary Key
            builder.HasKey(m => new { m.MovieId, m.DirectorId });

            // Properties Configuration


            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
