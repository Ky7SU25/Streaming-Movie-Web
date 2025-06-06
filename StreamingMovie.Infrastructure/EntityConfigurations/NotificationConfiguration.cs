using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Entity Configuration for Notification
    /// </summary>
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            // Table name
            builder.ToTable("Notification");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
