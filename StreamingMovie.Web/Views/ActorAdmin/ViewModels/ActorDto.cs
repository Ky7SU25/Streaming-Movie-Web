using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Web.Views.ActorAdmin.ViewModels
{
    public class ActorDto
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Fullname { get; set; }
        public string Biography { get; set; }
        public DateTime Dob { get; set; }
        public string Nationality { get; set; }
        public IFormFile? ImgProfile { get; set; }
    }
}
