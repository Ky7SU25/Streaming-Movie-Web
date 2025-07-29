namespace StreamingMovie.Application.DTOs
{
    public class RatingResponseDTO
    {
        // rating information
        public int Id { get; set; }
        public int RatingValue { get; set; }
        public string? Review { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsCurrentUserReview { get; set; }

        // user information
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string? AvatarUrl { get; set; }

        //movie
        public string Slug { get; set; }

    }
}
