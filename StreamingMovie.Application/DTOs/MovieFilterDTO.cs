using StreamingMovie.Domain.Enums;

namespace StreamingMovie.Application.DTOs
{
    public class MovieFilterDTO
    {
        public string? Keyword { get; set; }
        public IEnumerable<string>? Categories { get; set; }
        public IEnumerable<string>? Countries { get; set; }
        public IEnumerable<int>? Years { get; set; }
        public MovieType? Type { get; set; } = MovieType.All;
        public string? Status { get; set; }
        public MovieSortOption? OrderBy { get; set; } = MovieSortOption.Newest;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
