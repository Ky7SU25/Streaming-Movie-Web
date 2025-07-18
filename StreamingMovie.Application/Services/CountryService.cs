﻿using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.UnitOfWorks;

namespace StreamingMovie.Application.Services
{
    public class CountryService : GenericService<Country>, ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountryService(IUnitOfWork unitOfWork)
            : base(unitOfWork.CountryRepository)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
