using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for Episode
    /// </summary>
    public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
        {
            // Table name
            builder.ToTable("Episode");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(e => e.Id).HasColumnName("Id").HasColumnType("INT").IsRequired(true);
            builder
                .Property(e => e.SeriesId)
                .HasColumnName("SeriesId")
                .HasColumnType("INT")
                .IsRequired(true);
            builder
                .Property(e => e.SeasonNumber)
                .HasColumnName("SeasonNumber")
                .HasColumnType("INT")
                .IsRequired(true);
            builder
                .Property(e => e.EpisodeNumber)
                .HasColumnName("EpisodeNumber")
                .HasColumnType("INT")
                .IsRequired(true);
            builder
                .Property(e => e.Title)
                .HasColumnName("Title")
                .HasColumnType("VARCHAR(255)")
                .IsRequired(false);
            builder
                .Property(e => e.Description)
                .HasColumnName("Description")
                .HasColumnType("TEXT")
                .IsRequired(false);
            builder
                .Property(e => e.Duration)
                .HasColumnName("Duration")
                .HasColumnType("INT")
                .IsRequired(false);
            builder
                .Property(e => e.AirDate)
                .HasColumnName("AirDate")
                .HasColumnType("DATE")
                .IsRequired(false);
            builder
                .Property(e => e.ViewCount)
                .HasColumnName("ViewCount")
                .HasColumnType("INT")
                .IsRequired(false)
                .HasDefaultValue(0);
            builder
                .Property(e => e.IsPremium)
                .HasColumnName("IsPremium")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(false);
            builder
                .Property(e => e.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME")
                .IsRequired(false)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
