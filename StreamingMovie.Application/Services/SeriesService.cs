using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.UnitOfWorks;

namespace StreamingMovie.Application.Services
{
    public class SeriesService : GenericService<Series>, ISeriesService
    {
        public SeriesService(IUnitOfWork unitOfWork)
            : base(unitOfWork.SeriesRepository) { }
    }
}
