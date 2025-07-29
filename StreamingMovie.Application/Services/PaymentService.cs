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

        public async Task<(string[] Labels, decimal[] Data)> GetMonthlyChartDataAsync(int year)
        {
            var labels = Enumerable.Range(1, 12)
            .Select(m => new DateTime(year, m, 1).ToString("MMM"))
            .ToArray();

            var monthlyTotals = await _unitOfWork.PaymentRepository.GetMonthlyTotalsAsync(year);

            var data = Enumerable.Range(1, 12)
                .Select(m => monthlyTotals.ContainsKey(m) ? monthlyTotals[m] : 0)
                .ToArray();

            return (labels, data);
        }
    }
}
