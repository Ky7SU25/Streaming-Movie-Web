using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for SeriesCategory
    /// </summary>
    public class SeriesCategoryConfiguration : IEntityTypeConfiguration<SeriesCategory>
    {
        public void Configure(EntityTypeBuilder<SeriesCategory> builder)
        {
            // Table name
            builder.ToTable("SeriesCategory");

            // Primary Key
            builder.HasKey(s => new { s.SeriesId, s.CategoryId });

            // Properties Configuration


            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
