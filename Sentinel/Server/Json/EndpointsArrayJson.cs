namespace Sentinel.Server.Json
{
    using Newtonsoft.Json;

    public class EndpointsArrayJson
    {
        [JsonProperty("endpoints")]
        public EndpointJson[] Endpoints { get; set; }
    }

}
