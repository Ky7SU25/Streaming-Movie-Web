using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.UnitOfWorks;

namespace StreamingMovie.Application.Services
{
    public class MovieCategoryService: GenericService<MovieCategory>, IMovieCategoryService
    {
        
        private readonly IUnitOfWork _unitOfWork;


        public MovieCategoryService(IUnitOfWork unitOfWork)
            : base(unitOfWork.MovieCategoryRepository)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<CategoryViewCountDto>> GetViewsByMovieCategoryAsync()
        {
            return await _unitOfWork.MovieCategoryRepository.GetViewsByMovieCategoryAsync();
        }

        
    }
}
