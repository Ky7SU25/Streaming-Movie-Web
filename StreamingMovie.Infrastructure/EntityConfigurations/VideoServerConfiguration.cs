using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for VideoServer
    /// </summary>
    public class VideoServerConfiguration : IEntityTypeConfiguration<VideoServer>
    {
        public void Configure(EntityTypeBuilder<VideoServer> builder)
        {
            // Table name
            builder.ToTable("VideoServer");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(v => v.Id).HasColumnName("Id").HasColumnType("INT").IsRequired(true);
            builder
                .Property(v => v.Name)
                .HasColumnName("Name")
                .HasColumnType("VARCHAR(50)")
                .IsRequired(true);
            builder
                .Property(v => v.BaseUrl)
                .HasColumnName("BaseUrl")
                .HasColumnType("VARCHAR(255)")
                .IsRequired(true);
            builder
                .Property(v => v.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(true);
            builder
                .Property(v => v.Priority)
                .HasColumnName("Priority")
                .HasColumnType("INT")
                .IsRequired(false)
                .HasDefaultValue(1);
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
