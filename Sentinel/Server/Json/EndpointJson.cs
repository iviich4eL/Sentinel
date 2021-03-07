namespace Sentinel.Server.Json
{
    using System;
    using Newtonsoft.Json;

    public class EndpointJson
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("tf_sec")]
        public long TfSec { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
