namespace Sentinel.Server.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using Hubs;
    using Shared.Dto;
    using Shared;
    using Domain;

    public class PollingService : IDisposable
    {
        private readonly List<Timer> timers = new List<Timer>();

        private readonly IEndpointRepository endpointRepository;
        private readonly IHubContext<SentinelHub> sentinelHubContext;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<PollingService> logger;
        private readonly CurrentTimeFunc now;

        public PollingService(IEndpointRepository endpointRepository, 
            IHubContext<SentinelHub> sentinelHubContext, 
            IHttpClientFactory httpClientFactory, 
            ILogger<PollingService> logger, 
            CurrentTimeFunc now)
        {
            this.endpointRepository = endpointRepository;
            this.sentinelHubContext = sentinelHubContext;
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
            this.now = now;
        }

        public Task Start(CancellationToken stoppingToken)
        {
            logger.LogInformation("Start polling.");

            foreach (var (endpointId, endpoint) in endpointRepository.Endpoints)
            {
                var timer = new Timer(async x => await PollAsync(endpointId, endpoint, stoppingToken), 
                    null, TimeSpan.Zero, endpoint.Period);

                timers.Add(timer);
            }

            return Task.CompletedTask;
        }

        public async Task PollAsync(string endpointId, Endpoint endpoint, CancellationToken cancellationToken)
        {
            string message;
            var timestamp = now();

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, endpoint.Address);
                var httpClient = httpClientFactory.CreateClient();

                using var response = await httpClient.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                    await OnSuccess(endpointId, timestamp, response, cancellationToken);
                else
                    await OnError(endpointId, timestamp, "not success", cancellationToken);
            }
            catch (HttpRequestException exception)
            {
                message = "HttpRequestException when calling the API";
                logger.LogError(exception, message);

                await OnError(endpointId, timestamp, message, cancellationToken);
            }
            catch (TimeoutException exception)
            {
                message = "TimeoutException during call to API";
                logger.LogError(exception, message);

                await OnError(endpointId, timestamp, message, cancellationToken);

            }
            catch (Exception exception)
            {
                message = "Unhandled exception when calling the API";
                logger.LogError(exception, message);

                await OnError(endpointId, timestamp, message, cancellationToken);
            }
        }

        private async Task OnSuccess(string endpointId, DateTimeOffset timestamp, HttpResponseMessage httpResponseMessage, 
            CancellationToken cancellationToken)
        {
            var message = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

            var response = new ResponseDto
            {
                TimeStamp = timestamp,
                From = endpointId,
                Message = message
            };

            foreach (var connectionId in endpointRepository.GetConnections(endpointId))
                await sentinelHubContext
                    .Clients
                    .Client(connectionId.Key)
                    .SendAsync(Constants.ReceiveMessage, response, cancellationToken);
        }

        private async Task OnError(string endpointId, DateTimeOffset timestamp, string error, 
            CancellationToken cancellationToken)
        {
            var response = new ErrorDto
            {
                TimeStamp = timestamp,
                From = endpointId,
                Error = error
            };

            foreach (var connectionId in endpointRepository.GetConnections(endpointId))
                await sentinelHubContext
                    .Clients
                    .Client(connectionId.Key)
                    .SendAsync(Constants.ReceiveError, response, cancellationToken);
        }

        public Task Stop(CancellationToken stoppingToken)
        {
            logger.LogInformation("Timed Hosted Service is stopping.");

            foreach (var timer in timers)
                timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }


        public void Dispose()
        {
            foreach (var timer in timers)
                timer?.Dispose();
        }
    }
}
