namespace Sentinel.Server.HostedServices
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Services;

    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly EndpointsService endpointsService;
        private readonly PollingService pollingService;
        private readonly ILogger<TimedHostedService> logger;

        public TimedHostedService(EndpointsService endpointsService,
            PollingService pollingService,
            ILogger<TimedHostedService> logger)
        {
            this.endpointsService = endpointsService;
            this.pollingService = pollingService;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Timed Hosted Service running.");

            endpointsService.AddEndpoints();
            pollingService.Start(stoppingToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Timed Hosted Service is stopping.");
           
            pollingService.Stop(stoppingToken);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            pollingService.Dispose();
        }
    }
}