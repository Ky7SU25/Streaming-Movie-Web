using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingMovie.Application.DTOs
{
    public class MovieVideoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public string? VideoUrl { get; set; }
        public string? VideoServer { get; set; }
        public string? VideoQuality { get; set; }
        public string? SubtitleUrl { get; set; }
        public string? Language { get; set; }
        
        // Quality-specific URLs for HLS streaming
        public Dictionary<string, string> QualityUrls { get; set; } = new Dictionary<string, string>();
        public string? MasterPlaylistUrl { get; set; }
        
        // Available qualities for this movie
        public List<VideoQualityInfo> AvailableQualities { get; set; } = new List<VideoQualityInfo>();
    }

    public class VideoQualityInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Resolution { get; set; } = string.Empty;
        public int? Bitrate { get; set; }
        public string PlaylistUrl { get; set; } = string.Empty;
    }
}
