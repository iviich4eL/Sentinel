namespace Sentinel.Server
{
    using System.Collections.Concurrent;
    using System;

    public class Endpoint
    {
        public Endpoint(string address, TimeSpan period)
        {
            Address = address;
            Period = period;
            ConnectionIds = new ConcurrentDictionary<string, byte>();
        }

        public string Address { get; }

        public TimeSpan Period { get; }

        public ConcurrentDictionary<string, byte> ConnectionIds { get; }
    }
}
