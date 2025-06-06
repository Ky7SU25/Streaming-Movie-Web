using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for MovieVideo
    /// </summary>
    public class MovieVideoConfiguration : IEntityTypeConfiguration<MovieVideo>
    {
        public void Configure(EntityTypeBuilder<MovieVideo> builder)
        {
            // Table name
            builder.ToTable("MovieVideo");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
