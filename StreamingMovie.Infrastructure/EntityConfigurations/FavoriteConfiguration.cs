using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for Favorite
    /// </summary>
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            // Table name
            builder.ToTable("Favorite");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(f => f.Id).HasColumnName("Id").HasColumnType("INT").IsRequired(true);
            builder
                .Property(f => f.UserId)
                .HasColumnName("UserId")
                .HasColumnType("INT")
                .IsRequired(true);
            builder
                .Property(f => f.MovieId)
                .HasColumnName("MovieId")
                .HasColumnType("INT")
                .IsRequired(false);
            builder
                .Property(f => f.SeriesId)
                .HasColumnName("SeriesId")
                .HasColumnType("INT")
                .IsRequired(false);
            builder
                .Property(f => f.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME")
                .IsRequired(false)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
