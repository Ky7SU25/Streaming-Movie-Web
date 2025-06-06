using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for MovieCategory
    /// </summary>
    public class MovieCategoryConfiguration : IEntityTypeConfiguration<MovieCategory>
    {
        public void Configure(EntityTypeBuilder<MovieCategory> builder)
        {
            // Table name
            builder.ToTable("MovieCategory");

            // Primary Key
            builder.HasKey(e => new { e.MovieId, e.CategoryId });

            // Properties Configuration


            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
