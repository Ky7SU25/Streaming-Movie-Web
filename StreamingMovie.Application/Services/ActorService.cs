using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Domain.UnitOfWorks;

namespace StreamingMovie.Application.Services
{
    public class ActorService : GenericService<Actor>, IActorService
    {
        private readonly IUnitOfWork _unitOfWork;


        public ActorService(IUnitOfWork unitOfWork)
            : base(unitOfWork.ActorRepository)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
