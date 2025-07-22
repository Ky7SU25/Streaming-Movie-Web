using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamingMovie.Domain.UnitOfWorks;

namespace StreamingMovie.Application.Services
{
    public class AdminUploadService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminUploadService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
