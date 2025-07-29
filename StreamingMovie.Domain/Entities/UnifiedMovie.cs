using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace StreamingMovie.Domain.Entities
{
    public class UnifiedMovie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? OriginalTitle { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? PosterUrl { get; set; }
        public string? BannerUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public float? ImdbRating { get; set; }
        public float? OurRating { get; set; }
        public int? ViewCount { get; set; }
        public string? Status { get; set; }
        public bool? IsPremium { get; set; }
        public int? CountryId { get; set; }
        public bool IsSeries { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string EmbeddingJson { get; set; } // stringified float[]
        [NotMapped]
        public float[] Embedding
        {
            get => string.IsNullOrEmpty(EmbeddingJson) ? new float[0] : JsonSerializer.Deserialize<float[]>(EmbeddingJson);
            set => EmbeddingJson = JsonSerializer.Serialize(value);
        }
    }
}
