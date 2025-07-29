using StreamingMovie.Application.DTOs;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;

namespace StreamingMovie.Application.Interfaces
{
    public interface IMovieService : IGenericService<Movie>
    {
        Task<MovieVideoDTO> GetMovieVideoAsync(int movieId);

        Task<MovieVideoDTO> GetHighViewMovieVideoAsync();

        Task<List<TopViewMovieDTO>> GetTopViewMoviesAsync(string period = "day", int take = 4);
        Task<int> GetTotalMoviesAsync();
    }
}
