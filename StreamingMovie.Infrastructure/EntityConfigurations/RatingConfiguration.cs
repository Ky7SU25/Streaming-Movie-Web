using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for Rating
    /// </summary>
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            // Table name
            builder.ToTable("Rating");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
