using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for UnifiedMovie
    /// </summary>
    public class UnifiedMovieConfiguration : IEntityTypeConfiguration<UnifiedMovie>
    {
        public void Configure(EntityTypeBuilder<UnifiedMovie> builder)
        {
            // View mapping
            builder.ToView("UnifiedMovie");
            builder.HasNoKey(); 
        }
    }
}