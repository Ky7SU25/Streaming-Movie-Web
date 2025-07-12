using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.UnitOfWorks;

namespace StreamingMovie.Application.Services
{
    public class CategoryService : GenericService<Category>, ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
            : base(unitOfWork.CategoryRepository)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Category?> GetBySlugAsync(string slug)
        {
            return await _unitOfWork.CategoryRepository.GetBySlugAsync(slug);
        }
    }
}
