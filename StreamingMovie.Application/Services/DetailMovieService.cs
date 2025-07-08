using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.UnitOfWorks;

namespace StreamingMovie.Application.Services
{
    public class DetailMovieService 
    {
        private readonly IUnitOfWork _unitOfWork;
        public DetailMovieService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Movie> GetMovieByIdAsync(int id)
        {
            if (id == null)
            {
                return Task.FromResult<Movie>(null);
            }
            return _unitOfWork.MovieRepository.GetByIdAsync(id);
        }
    }
}
