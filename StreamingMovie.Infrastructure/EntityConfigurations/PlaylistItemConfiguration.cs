using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for PlaylistItem
    /// </summary>
    public class PlaylistItemConfiguration : IEntityTypeConfiguration<PlaylistItem>
    {
        public void Configure(EntityTypeBuilder<PlaylistItem> builder)
        {
            // Table name
            builder.ToTable("PlaylistItem");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(p => p.Id).HasColumnName("Id").HasColumnType("INT").IsRequired(true);
            builder
                .Property(p => p.PlaylistId)
                .HasColumnName("PlaylistId")
                .HasColumnType("INT")
                .IsRequired(true);
            builder
                .Property(p => p.MovieId)
                .HasColumnName("MovieId")
                .HasColumnType("INT")
                .IsRequired(false);
            builder
                .Property(p => p.SeriesId)
                .HasColumnName("SeriesId")
                .HasColumnType("INT")
                .IsRequired(false);
            builder
                .Property(p => p.Position)
                .HasColumnName("Position")
                .HasColumnType("INT")
                .IsRequired(true);
            builder
                .Property(p => p.AddedAt)
                .HasColumnName("AddedAt")
                .HasColumnType("DATETIME")
                .IsRequired(false)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
