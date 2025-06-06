using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for Series
    /// </summary>
    public class SeriesConfiguration : IEntityTypeConfiguration<Series>
    {
        public void Configure(EntityTypeBuilder<Series> builder)
        {
            // Table name
            builder.ToTable("Series");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration


            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
