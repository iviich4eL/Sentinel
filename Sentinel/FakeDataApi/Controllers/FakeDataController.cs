namespace FakeDataApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("data")]
    public class FakeDataController : ControllerBase
    {
        [HttpGet]
        public Task<string> Test() => Task.FromResult("Test");


        [HttpGet("v1/number")]
        public Task<int> GetRandomNumber()
        {
            var random = new Random();
            var number = random.Next(10,19);

            return Task.FromResult(number);
        }

        [HttpGet("v2/number")] 
        public Task<int> GetRandomNumber2()
        {
            var random = new Random();
            var number = random.Next(20,29);

            return Task.FromResult(number);
        }

        [HttpGet("v3/number")]
        public Task<int> GetRandomNumber3()
        {
            var random = new Random();
            var number = random.Next(30,39);

            return Task.FromResult(number);
        }

        [HttpGet("v4/number")]
        public Task<int> GetRandomNumber4()
        {
            var random = new Random();
            var number = random.Next(40,49);

            return Task.FromResult(number);
        }

        [HttpGet("v5/number")]
        public Task<int> GetRandomNumber5()
        {
            var random = new Random();
            var number = random.Next(50,59);

            return Task.FromResult(number);
        }

        [HttpGet("v6/number")]
        public Task<int> GetRandomNumber6()
        {
            var random = new Random();
            var number = random.Next(60, 69);

            return Task.FromResult(number);
        }

        [HttpGet("v7/number")]
        public Task<int> GetRandomNumber7()
        {
            var random = new Random();
            var number = random.Next(70, 79);

            return Task.FromResult(number);
        }

        [HttpGet("v8/number")]
        public Task<int> GetRandomNumber8()
        {
            var random = new Random();
            var number = random.Next(80, 89);

            return Task.FromResult(number);
        }

        [HttpGet("v9/number")]
        public Task<int> GetRandomNumber9()
        {
            var random = new Random();
            var number = random.Next(90, 99);

            return Task.FromResult(number);
        }

        [HttpGet("v10/number")]
        public Task<int> GetRandomNumber10()
        {
            var random = new Random();
            var number = random.Next(100, 109);

            return Task.FromResult(number);
        }
    }
}
