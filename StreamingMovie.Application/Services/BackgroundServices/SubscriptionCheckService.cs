using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StreamingMovie.Application.Interfaces;

namespace StreamingMovie.Application.Services.BackgroundServices
{
    public class SubscriptionCheckService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public SubscriptionCheckService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWorkAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            var expiredUsers = await userService.GetPremiumUsersAsync();

            foreach (var user in expiredUsers)
            {
                user.SubscriptionType = "Basic";
                user.SubscriptionStartDate = null;
                user.SubscriptionEndDate = null;
                userService.UpdateAsync(user);
            }

        }
    }
}
