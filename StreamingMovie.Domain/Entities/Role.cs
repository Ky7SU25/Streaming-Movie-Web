using Microsoft.AspNetCore.Identity;

namespace StreamingMovie.Domain.Entities;

/// <summary>
/// Represent the "Roles" table.
/// </summary>
public class Role : IdentityRole<int> { }
