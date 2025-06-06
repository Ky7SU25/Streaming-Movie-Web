using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for Banner
    /// </summary>
    public class BannerConfiguration : IEntityTypeConfiguration<Banner>
    {
        public void Configure(EntityTypeBuilder<Banner> builder)
        {
            // Table name
            builder.ToTable("Banner");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(b => b.Id).HasColumnName("Id").HasColumnType("INT").IsRequired(true);
            builder
                .Property(b => b.Title)
                .HasColumnName("Title")
                .HasColumnType("VARCHAR(255)")
                .IsRequired(true);
            builder
                .Property(b => b.ImageUrl)
                .HasColumnName("ImageUrl")
                .HasColumnType("VARCHAR(255)")
                .IsRequired(true);
            builder
                .Property(b => b.LinkUrl)
                .HasColumnName("LinkUrl")
                .HasColumnType("VARCHAR(255)")
                .IsRequired(false);
            builder
                .Property(b => b.MovieId)
                .HasColumnName("MovieId")
                .HasColumnType("INT")
                .IsRequired(false);
            builder
                .Property(b => b.SeriesId)
                .HasColumnName("SeriesId")
                .HasColumnType("INT")
                .IsRequired(false);
            builder
                .Property(b => b.Position)
                .HasColumnName("Position")
                .HasColumnType("INT")
                .IsRequired(false)
                .HasDefaultValue(1);
            builder
                .Property(b => b.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(true);
            builder
                .Property(b => b.StartDate)
                .HasColumnName("StartDate")
                .HasColumnType("DATE")
                .IsRequired(false);
            builder
                .Property(b => b.EndDate)
                .HasColumnName("EndDate")
                .HasColumnType("DATE")
                .IsRequired(false);
            builder
                .Property(b => b.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME")
                .IsRequired(false)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
