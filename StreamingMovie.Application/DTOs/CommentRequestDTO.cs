namespace StreamingMovie.Application.DTOs
{
    public class CommentRequestDTO
    {
        public int UserId { get; set; }
        public string Content { get; set; }
        public int? MovieId { get; set; } // For movie comments
        public int? EpisodeId { get; set; } // For series comments
        public int? ParentId { get; set; }
    }
}