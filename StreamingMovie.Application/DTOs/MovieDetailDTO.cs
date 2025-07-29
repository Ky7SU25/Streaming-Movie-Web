using StreamingMovie.Application.Common.Pagination;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Application.DTOs
{
    public class MovieDetailDTO : UnifiedMovieDTO
    {
        public string? Language { get; set; }
        public IEnumerable<string>? Genres { get; set; }
        public IEnumerable<Movie>? Movies { get; set; }
        public PagedResult<RatingResponseDTO>? Ratings { get; set; }
        // For current user
        public bool IsUserFavorite { get; set; }
        public RatingResponseDTO? UserReview { get; set; }

    }
}
