using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;

namespace StreamingMovie.Application.Interfaces
{
    public interface IPaymentService : IGenericService<Payment>
    {
         Task<(string[] Labels, decimal[] Data)> GetMonthlyChartDataAsync(int year);
        Task<float> GetTotalRevenueAsync();
    }
}
