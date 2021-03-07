namespace FakeDataApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("unhealthy")]
    public class UnhealthyController : Controller
    {
        // http://localhost:5002/unhealthy/v1/slow
        [HttpGet("v1/slow")]
        public Task<int> GetRandomNumberSlow()
        {
            var random = new Random();
            var number = random.Next(0, 100);

            Task.Delay(5000).Wait();

            return Task.FromResult(number);
        }


        [HttpGet("v1/exception")]
        public Task<int> GetException()
        {
            throw new Exception("Some message");
        }
    }
}
