using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Web.Views.UserProfile.ViewModels
{
    public class UserProfileViewModel
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
    }
}
