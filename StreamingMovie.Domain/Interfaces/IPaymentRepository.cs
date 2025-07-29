using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Domain.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment> 
    {
        Task<IEnumerable<Payment>> GetPaymentsInMonthAsync(int year, int month);
        Task<Dictionary<int, decimal>> GetMonthlyTotalsAsync(int year);
    }
    
}
