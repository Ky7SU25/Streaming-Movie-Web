using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.EntityConfigurations;

/// <summary>
/// Represents configuration of "Roles" table.
/// </summary>
internal class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder) { }
}
