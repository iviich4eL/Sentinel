namespace Sentinel.Server
{
    using System.Collections.Concurrent;

    // Todo: add auth, one user per multiple connections
    public class ConnectionMapper<T>
    {
        private readonly ConcurrentDictionary<T, string> connections =
            new ConcurrentDictionary<T, string>();

        public int Count => connections.Count;

        public string GetConnections(T key) =>
            connections.TryGetValue(key, out var connectionValues)
                ? connectionValues
                : string.Empty;

        public bool Add(T key, string connectionId) => 
            connections.TryAdd(key, connectionId);

        public void Remove(T key) =>
            connections.TryRemove(key, out var connectionId);
    }
}