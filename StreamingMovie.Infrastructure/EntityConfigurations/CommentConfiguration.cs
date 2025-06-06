using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for Comment
    /// </summary>
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            // Table name
            builder.ToTable("Comment");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(c => c.Id).HasColumnName("Id").HasColumnType("INT").IsRequired(true);
            builder
                .Property(c => c.UserId)
                .HasColumnName("UserId")
                .HasColumnType("INT")
                .IsRequired(true);
            builder
                .Property(c => c.MovieId)
                .HasColumnName("MovieId")
                .HasColumnType("INT")
                .IsRequired(false);
            builder
                .Property(c => c.SeriesId)
                .HasColumnName("SeriesId")
                .HasColumnType("INT")
                .IsRequired(false);
            builder
                .Property(c => c.EpisodeId)
                .HasColumnName("EpisodeId")
                .HasColumnType("INT")
                .IsRequired(false);
            builder
                .Property(c => c.ParentId)
                .HasColumnName("ParentId")
                .HasColumnType("INT")
                .IsRequired(false);
            builder
                .Property(c => c.Content)
                .HasColumnName("Content")
                .HasColumnType("TEXT")
                .IsRequired(true);
            builder
                .Property(c => c.LikeCount)
                .HasColumnName("LikeCount")
                .HasColumnType("INT")
                .IsRequired(false)
                .HasDefaultValue(0);
            builder
                .Property(c => c.DislikeCount)
                .HasColumnName("DislikeCount")
                .HasColumnType("INT")
                .IsRequired(false)
                .HasDefaultValue(0);
            builder
                .Property(c => c.IsApproved)
                .HasColumnName("IsApproved")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(true);
            builder
                .Property(c => c.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME")
                .IsRequired(false)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder
                .Property(c => c.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .HasColumnType("DATETIME")
                .IsRequired(false)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
