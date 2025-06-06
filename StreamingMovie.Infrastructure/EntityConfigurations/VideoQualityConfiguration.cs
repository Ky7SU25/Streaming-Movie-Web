using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for VideoQuality
    /// </summary>
    public class VideoQualityConfiguration : IEntityTypeConfiguration<VideoQuality>
    {
        public void Configure(EntityTypeBuilder<VideoQuality> builder)
        {
            // Table name
            builder.ToTable("VideoQuality");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(v => v.Id).HasColumnName("Id").HasColumnType("INT").IsRequired(true);
            builder
                .Property(v => v.Name)
                .HasColumnName("Name")
                .HasColumnType("VARCHAR(20)")
                .IsRequired(true);
            builder
                .Property(v => v.Resolution)
                .HasColumnName("Resolution")
                .HasColumnType("VARCHAR(20)")
                .IsRequired(true);
            builder
                .Property(v => v.Bitrate)
                .HasColumnName("Bitrate")
                .HasColumnType("INT")
                .IsRequired(false);
            builder
                .Property(v => v.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME")
                .IsRequired(false)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
