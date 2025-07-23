using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Web.Views.ActorAdmin.ViewModels
{
    public class CategoryDto
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public string Slug { get; set; }
        public string? Description { get; set; }
        
    }
}
