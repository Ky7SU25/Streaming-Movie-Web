using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for MovieActor
    /// </summary>
    public class MovieActorConfiguration : IEntityTypeConfiguration<MovieActor>
    {
        public void Configure(EntityTypeBuilder<MovieActor> builder)
        {
            // Table name
            builder.ToTable("MovieActor");

            // Properties Configuration

            // Primary Key
            builder.HasKey(e => new { e.MovieId, e.ActorId });

            builder
                .Property(m => m.CharacterName)
                .HasColumnName("CharacterName")
                .HasColumnType("VARCHAR(100)")
                .IsRequired(false);
            builder
                .Property(m => m.IsMainCharacter)
                .HasColumnName("IsMainCharacter")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(false);
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
