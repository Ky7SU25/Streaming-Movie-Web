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
    public class PaymentService : GenericService<Payment>, IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;


        public PaymentService(IUnitOfWork unitOfWork)
            : base(unitOfWork.PaymentRepository)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
