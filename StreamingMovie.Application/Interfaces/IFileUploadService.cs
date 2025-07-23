using Microsoft.AspNetCore.Http;

namespace StreamingMovie.Application.Interfaces
{
    /// <summary>
    /// Placeholder interface cho file upload service
    /// </summary>
    public interface IFileUploadService
    {
        Task<object> UploadFileAsync(IFormFile file, string fileType);
        Task<object> UploadChunkAsync(IFormFile chunkFile, string fileName, int chunkNumber, int totalChunks, string chunkId);
        Task<object> GetSignedUrlAsync(string fileName, string fileType, string contentType, long fileSize);
    }
}