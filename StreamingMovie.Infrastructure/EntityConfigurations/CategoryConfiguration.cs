using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for Category
    /// </summary>
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Table name
            builder.ToTable("Category");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(c => c.Id).HasColumnName("Id").HasColumnType("INT").IsRequired(true);
            builder
                .Property(c => c.Name)
                .HasColumnName("Name")
                .HasColumnType("VARCHAR(100)")
                .IsRequired(true);
            builder
                .Property(c => c.Description)
                .HasColumnName("Description")
                .HasColumnType("TEXT")
                .IsRequired(false);
            builder
                .Property(c => c.Slug)
                .HasColumnName("Slug")
                .HasColumnType("VARCHAR(100)")
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
