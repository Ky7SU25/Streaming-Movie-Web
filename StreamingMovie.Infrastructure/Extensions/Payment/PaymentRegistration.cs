using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamingMovie.Application.Interfaces.ExternalServices.VNpay;
using StreamingMovie.Infrastructure.ExternalServices.Mail;
using StreamingMovie.Infrastructure.ExternalServices.VNpay;

namespace StreamingMovie.Infrastructure.Extensions.Payment
{
    public static class PaymentRegistration
    {
        public static IServiceCollection AddPayment(
        this IServiceCollection services,
        IConfiguration config
    )
        {

            services.AddTransient<IVnPayService, VNpayHandle>();
            return services;
        }
    }
}
