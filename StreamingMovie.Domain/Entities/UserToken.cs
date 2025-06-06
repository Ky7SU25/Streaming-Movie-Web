using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace StreamingMovie.Domain.Entities;

/// <summary>
/// Represent the "UserTokens" table.
/// </summary>
public class UserToken : IdentityUserToken<int>
{
    public DateTime ExpiredAt { get; set; }

    // Navigation properties.
    public User User { get; set; }
}
