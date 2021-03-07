namespace Sentinel.Server.Domain
{
    using System.Collections.Concurrent;

    public interface IEndpointRepository
    {
        ConcurrentDictionary<string, Endpoint> Endpoints { get; }
        ConcurrentDictionary<string, byte> GetConnections(string endpointId);
    }
}
