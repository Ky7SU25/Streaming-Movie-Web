using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for Actor
    /// </summary>
    public class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            // Table name
            builder.ToTable("Actor");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(a => a.Id).HasColumnName("Id").HasColumnType("INT").IsRequired(true);
            builder
                .Property(a => a.Name)
                .HasColumnName("Name")
                .HasColumnType("VARCHAR(100)")
                .IsRequired(true);
            builder
                .Property(a => a.Biography)
                .HasColumnName("Biography")
                .HasColumnType("TEXT")
                .IsRequired(false);
            builder
                .Property(a => a.DateOfBirth)
                .HasColumnName("DateOfBirth")
                .HasColumnType("DATE")
                .IsRequired(false);
            builder
                .Property(a => a.Nationality)
                .HasColumnName("Nationality")
                .HasColumnType("VARCHAR(50)")
                .IsRequired(false);
            builder
                .Property(a => a.AvatarUrl)
                .HasColumnName("AvatarUrl")
                .HasColumnType("VARCHAR(255)")
                .IsRequired(false);
            builder
                .Property(a => a.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME")
                .IsRequired(false)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
