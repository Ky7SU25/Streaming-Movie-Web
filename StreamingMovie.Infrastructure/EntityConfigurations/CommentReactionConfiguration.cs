using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for CommentReaction
    /// </summary>
    public class CommentReactionConfiguration : IEntityTypeConfiguration<CommentReaction>
    {
        public void Configure(EntityTypeBuilder<CommentReaction> builder)
        {
            // Table name
            builder.ToTable("CommentReaction");

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
                .Property(c => c.CommentId)
                .HasColumnName("CommentId")
                .HasColumnType("INT")
                .IsRequired(true);
            builder
                .Property(c => c.ReactionType)
                .HasColumnName("ReactionType")
                .HasColumnType("VARCHAR(10)")
                .IsRequired(true);
            builder
                .Property(c => c.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME")
                .IsRequired(false)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
