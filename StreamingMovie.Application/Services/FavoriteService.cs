using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.UnitOfWorks;

namespace StreamingMovie.Application.Services
{
    public class FavoriteService : GenericService<Favorite>, IFavoriteService
    {
        public FavoriteService(IUnitOfWork unitOfWork)
            : base(unitOfWork.FavoriteRepository) { }
    }
}
