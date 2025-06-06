using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for Movie
    /// </summary>
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            // Table name
            builder.ToTable("Movie");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration


            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
