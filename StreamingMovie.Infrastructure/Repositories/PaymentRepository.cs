using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Infrastructure.Data;

namespace StreamingMovie.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(MovieDbContext context)
           : base(context) { }

        public async Task<Dictionary<int, decimal>> GetMonthlyTotalsAsync(int year)
        {
            var result = await _dbSet
            .Where(p => p.PaymentCreateDate.Year == year)
            .GroupBy(p => p.PaymentCreateDate.Month)
            .Select(g => new
            {
                Month = g.Key,
                Total = g.Sum(p => p.Amount)
            })
            .ToListAsync();

            return result.ToDictionary(x => x.Month, x => x.Total);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsInMonthAsync(int year, int month)
        {
            return await _dbSet.Where(p => p.PaymentCreateDate.Year == year && p.PaymentCreateDate.Month == month)
            .ToListAsync();
        }

    }
}
