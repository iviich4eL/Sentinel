using Sentinel.Server.Domain;

namespace Sentinel.Server.Hubs
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using System;
    using Microsoft.Extensions.Logging;

    public class SentinelHub : Hub
    {
        private readonly IEndpointRepository endpointRepository;
        private readonly ILogger<SentinelHub> logger;

        public SentinelHub(IEndpointRepository endpointRepository, ILogger<SentinelHub> logger)
        {
            this.endpointRepository = endpointRepository;
            this.logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            foreach (var (_, endpoint) in endpointRepository.Endpoints)
                endpoint.ConnectionIds.TryAdd(Context.ConnectionId, byte.MinValue);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (exception != null)
                logger.LogError(exception.Message);

            foreach (var (endpointId, endpoint) in endpointRepository.Endpoints)
                endpoint.ConnectionIds.TryRemove(Context.ConnectionId, out _);

            return base.OnDisconnectedAsync(exception);
        }

        public Task Subscribe(string endpointId)
        {
            var connectionId = Context.ConnectionId;

            if (endpointRepository.Endpoints.TryGetValue(endpointId, out var endpoint))
                if (endpoint.ConnectionIds.TryAdd(connectionId, byte.MinValue))
                    return Task.CompletedTask;

            logger.LogWarning($"Subscribe error: {endpointId}");
            return Task.CompletedTask;
        }

        public Task Unsubscribe(string endpointId)
        {
            var connectionId = Context.ConnectionId;

            if (endpointRepository.Endpoints.TryGetValue(endpointId, out var endpoint))
                if (endpoint.ConnectionIds.TryRemove(connectionId, out _))
                    return Task.CompletedTask;

            logger.LogWarning($"Unsubscribe error: {endpointId}");
            return Task.CompletedTask;
        }
    }
}
