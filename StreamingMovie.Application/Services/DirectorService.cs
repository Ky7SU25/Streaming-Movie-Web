using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.UnitOfWorks;

namespace StreamingMovie.Application.Services
{
    internal class DirectorService : GenericService<Director>, IDirectorService
    {
          private readonly IUnitOfWork _unitOfWork;
        public DirectorService(IUnitOfWork unitOfWork)
            : base(unitOfWork.DirectorRepository)
        {
            _unitOfWork = unitOfWork;
        }

    }
}
