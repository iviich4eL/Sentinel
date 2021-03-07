namespace Sentinel.Server.Repositories
{
    using Domain;
    using System.Collections.Concurrent;

    public class EndpointRepository : IEndpointRepository
    {
        private static readonly ConcurrentDictionary<string, byte> Empty =
            new ConcurrentDictionary<string, byte>();

        public ConcurrentDictionary<string, Endpoint> Endpoints { get; } =
            new ConcurrentDictionary<string, Endpoint>();

        public ConcurrentDictionary<string, byte> GetConnections(string endpointId) =>
            Endpoints.TryGetValue(endpointId, out var endpoint)
                ? endpoint.ConnectionIds
                : Empty;
    }
}
