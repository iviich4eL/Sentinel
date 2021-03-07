namespace Sentinel.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Moq.Protected;
    using Server;
    using Server.Domain;
    using Server.Hubs;
    using Server.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SentinelServerTests
    {
        private static Mock<IEndpointRepository> endpointRepository = new Mock<IEndpointRepository>();
        private static Mock<IHubContext<SentinelHub>> hubContextSentinelMock = new Mock<IHubContext<SentinelHub>>();
        private static Mock<IHttpClientFactory> httpClientFactoryMock = new Mock<IHttpClientFactory>();
        private static Mock<ILogger<PollingService>> loggerMock = new Mock<ILogger<PollingService>>();

        [TestMethod]
        public async Task PollingService_EmptyEndpointAddress_NoExceptions()
        {
            var endpoints = CreateWithEmptyAddress();
            endpointRepository.Setup(x => x.Endpoints).Returns(endpoints);
                
            foreach (var (endpointId, endpoint) in endpointRepository.Object.Endpoints)
            {
                endpointRepository.Setup(x => x.GetConnections(endpointId)).Returns(endpoint.ConnectionIds);
            }

            httpClientFactoryMock = SetupHttpClientReturnHttpRequestException();

            var pollingService = new PollingService(endpointRepository.Object, hubContextSentinelMock.Object,
                httpClientFactoryMock.Object, loggerMock.Object, () => DateTimeOffset.Now);
                
            foreach (var (endpointId, endpoint) in endpointRepository.Object.Endpoints)
            {
                await pollingService.PollAsync(endpointId, endpoint, CancellationToken.None);
            }
        }

        private static ConcurrentDictionary<string, Endpoint> CreateWithEmptyAddress()
        {
            var endpoints = new ConcurrentDictionary<string, Endpoint>();
            endpoints.TryAdd("1", new Endpoint("https://google.com", TimeSpan.FromSeconds(1))); //Need some valid address
            //endpoints.TryAdd("2", new Endpoint("", TimeSpan.FromSeconds(1)));

            return endpoints;
        }

        private Mock<IHttpClientFactory> SetupHttpClientReturnHttpRequestException()
        {
            var message = "TEST MESSAGE No connection could be made because the target machine actively refused it";

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Throws(new HttpRequestException(message));

            var httpClient = new HttpClient(mockMessageHandler.Object);

            httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            return httpClientFactoryMock;
        }

        [TestMethod]
        public async Task PollingService_HttpOkButNoAnyConnectionIds_NoExceptions()
        {
            var endpoints = CreateWithEmptyAddress();
            endpointRepository.Setup(x => x.Endpoints).Returns(endpoints);

            foreach (var (endpointId, endpoint) in endpointRepository.Object.Endpoints)
            {
                endpointRepository.Setup(x => x.GetConnections(endpointId)).Returns(endpoint.ConnectionIds);
            }

            httpClientFactoryMock = SetupHttpClientReturnOk();

            var pollingService = new PollingService(endpointRepository.Object, hubContextSentinelMock.Object,
                httpClientFactoryMock.Object, loggerMock.Object, () => DateTimeOffset.Now);

            foreach (var (endpointId, endpoint) in endpointRepository.Object.Endpoints)
            {
                await pollingService.PollAsync(endpointId, endpoint, CancellationToken.None);
            }
        }


        private Mock<IHttpClientFactory> SetupHttpClientReturnOk()
        {
            var message = "Some content";

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(message)
                });

            var httpClient = new HttpClient(mockMessageHandler.Object);

            httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            return httpClientFactoryMock;
        }
    }
}