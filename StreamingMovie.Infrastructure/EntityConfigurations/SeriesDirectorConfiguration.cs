using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for SeriesDirector
    /// </summary>
    public class SeriesDirectorConfiguration : IEntityTypeConfiguration<SeriesDirector>
    {
        public void Configure(EntityTypeBuilder<SeriesDirector> builder)
        {
            // Table name
            builder.ToTable("SeriesDirector");

            // Primary Key
            builder.HasKey(s => new { s.SeriesId, s.DirectorId });

            // Properties Configuration


            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
