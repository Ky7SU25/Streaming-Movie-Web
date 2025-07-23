using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Application.DTOs
{
    public class RatingRequestDTO
    {
        public int? UserId { get; set; }
        public int? Id { get; set; } // Optional for updates
        public string? Slug { get; set; } // Optional for creates
        public int RatingValue { get; set; }
        public string? Review { get; set; }
    }
}
