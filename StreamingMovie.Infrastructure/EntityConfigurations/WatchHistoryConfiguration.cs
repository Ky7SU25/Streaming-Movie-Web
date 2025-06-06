using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for WatchHistory
    /// </summary>
    public class WatchHistoryConfiguration : IEntityTypeConfiguration<WatchHistory>
    {
        public void Configure(EntityTypeBuilder<WatchHistory> builder)
        {
            // Table name
            builder.ToTable("WatchHistory");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(w => w.Id).HasColumnName("Id").HasColumnType("INT").IsRequired(true);
            builder
                .Property(w => w.UserId)
                .HasColumnName("UserId")
                .HasColumnType("INT")
                .IsRequired(true);
            builder
                .Property(w => w.MovieId)
                .HasColumnName("MovieId")
                .HasColumnType("INT")
                .IsRequired(false);
            builder
                .Property(w => w.EpisodeId)
                .HasColumnName("EpisodeId")
                .HasColumnType("INT")
                .IsRequired(false);
            builder
                .Property(w => w.WatchPosition)
                .HasColumnName("WatchPosition")
                .HasColumnType("INT")
                .IsRequired(false)
                .HasDefaultValue(0);
            builder
                .Property(w => w.WatchDuration)
                .HasColumnName("WatchDuration")
                .HasColumnType("INT")
                .IsRequired(false)
                .HasDefaultValue(0);
            builder
                .Property(w => w.IsCompleted)
                .HasColumnName("IsCompleted")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(false);
            builder
                .Property(w => w.LastWatchedAt)
                .HasColumnName("LastWatchedAt")
                .HasColumnType("DATETIME")
                .IsRequired(false)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
