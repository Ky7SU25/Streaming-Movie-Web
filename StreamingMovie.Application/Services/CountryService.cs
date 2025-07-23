using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.UnitOfWorks;

namespace StreamingMovie.Application.Services
{
    public class CountryService : GenericService<Country>, ICountryService
    {
        public CountryService(IUnitOfWork unitOfWork)
            : base(unitOfWork.CountryRepository) { }
    }
}
