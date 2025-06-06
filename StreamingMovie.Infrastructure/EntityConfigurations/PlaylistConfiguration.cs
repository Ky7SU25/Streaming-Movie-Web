using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for Playlist
    /// </summary>
    public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            // Table name
            builder.ToTable("Playlist");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(p => p.Id).HasColumnName("Id").HasColumnType("INT").IsRequired(true);
            builder
                .Property(p => p.UserId)
                .HasColumnName("UserId")
                .HasColumnType("INT")
                .IsRequired(true);
            builder
                .Property(p => p.Name)
                .HasColumnName("Name")
                .HasColumnType("VARCHAR(255)")
                .IsRequired(true);
            builder
                .Property(p => p.Description)
                .HasColumnName("Description")
                .HasColumnType("TEXT")
                .IsRequired(false);
            builder
                .Property(p => p.IsPublic)
                .HasColumnName("IsPublic")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(false);
            builder
                .Property(p => p.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME")
                .IsRequired(false)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder
                .Property(p => p.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .HasColumnType("DATETIME")
                .IsRequired(false)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
