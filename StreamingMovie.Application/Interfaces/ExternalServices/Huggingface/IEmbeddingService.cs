namespace StreamingMovie.Application.Interfaces.ExternalServices.Huggingface;

public interface IEmbeddingService
{
    Task<float[]> GetEmbeddingAsync(string text);
}
