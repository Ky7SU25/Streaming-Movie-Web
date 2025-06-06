using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace StreamingMovie.Domain.Entities;

/// <summary>
/// Represents a user in the application.
/// </summary>
public class User : IdentityUser<int>
{
    public string FullName { get; set; }

    [MaxLength(255)]
    public string? AvatarUrl { get; set; }

    [MaxLength(10)]
    public string? SubscriptionType { get; set; }

    public DateTime? SubscriptionStartDate { get; set; }

    public DateTime? SubscriptionEndDate { get; set; }

    public bool? IsActive { get; set; } = true;

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; } = DateTime.Now;

    // Navigation Properties
    public virtual IEnumerable<WatchHistory> WatchHistories { get; set; }
    public virtual IEnumerable<Favorite> Favorites { get; set; }
    public virtual IEnumerable<Rating> Ratings { get; set; }
    public virtual IEnumerable<Comment> Comments { get; set; }
    public virtual IEnumerable<CommentReaction> CommentReactions { get; set; }
    public virtual IEnumerable<Playlist> Playlists { get; set; }
    public virtual IEnumerable<Notification> Notifications { get; set; }

    public IEnumerable<UserRole> UserRoles { get; set; }

    public IEnumerable<UserToken> UserTokens { get; set; }
}
