using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for SeriesActor
    /// </summary>
    public class SeriesActorConfiguration : IEntityTypeConfiguration<SeriesActor>
    {
        public void Configure(EntityTypeBuilder<SeriesActor> builder)
        {
            // Table name
            builder.ToTable("SeriesActor");

            // Primary Key
            builder.HasKey(e => new { e.SeriesId, e.ActorId });
            // Properties Configuration

            builder
                .Property(s => s.CharacterName)
                .HasColumnName("CharacterName")
                .HasColumnType("VARCHAR(100)")
                .IsRequired(false);
            builder
                .Property(s => s.IsMainCharacter)
                .HasColumnName("IsMainCharacter")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(false);
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
