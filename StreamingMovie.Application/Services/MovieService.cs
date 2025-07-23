using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.UnitOfWorks;

namespace StreamingMovie.Application.Services
{
    public class MovieService : GenericService<Movie>, IMovieService
    {
        public MovieService(IUnitOfWork unitOfWork)
            : base(unitOfWork.MovieRepository) { }
    }
}