namespace StreamingMovie.Web.Views.UserProfile.ViewModels
{
    public class EditUserViewModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public IFormFile? AvatarUrl { get; set; }
        public string CurrentAvatar { get; set; }
    }
}
