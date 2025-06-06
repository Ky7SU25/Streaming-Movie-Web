using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for EpisodeVideo
    /// </summary>
    public class EpisodeVideoConfiguration : IEntityTypeConfiguration<EpisodeVideo>
    {
        public void Configure(EntityTypeBuilder<EpisodeVideo> builder)
        {
            // Table name
            builder.ToTable("EpisodeVideo");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration


            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
