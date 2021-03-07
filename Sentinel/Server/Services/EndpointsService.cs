namespace Sentinel.Server.Services
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using Json;
    using Domain;

    public class EndpointsService
    {
        private readonly IEndpointRepository endpointRepository;

        public EndpointsService(IEndpointRepository endpointRepository)
        {
            this.endpointRepository = endpointRepository;
        }

        public Task AddEndpoints()
        {
            var jsonString = File.ReadAllText("endpoints.json");
            var endpointsArrayJson = JsonConvert.DeserializeObject<EndpointsArrayJson>(jsonString);

            foreach (var endpointJson in endpointsArrayJson.Endpoints)
            {
                var endpoint = 
                    new Endpoint(
                        address: endpointJson.Url.ToString(),
                        period: TimeSpan.FromSeconds(endpointJson.TfSec));

                endpointRepository.Endpoints.TryAdd(endpointJson.Id, endpoint);
            }

            return Task.CompletedTask;
        }
    }
}
