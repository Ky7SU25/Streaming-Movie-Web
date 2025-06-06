using Microsoft.AspNetCore.Identity;

namespace StreamingMovie.Domain.Entities;

/// <summary>
/// Represent the "UserRoles" table.
/// </summary>
public class UserRole : IdentityUserRole<int>
{
    // Navigation properties.
    public User User { get; set; }

    public Role Role { get; set; }
}
