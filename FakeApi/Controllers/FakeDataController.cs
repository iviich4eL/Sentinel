using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeApi.Controllers
{
    [ApiController]
    [Route("data")]
    public class FakeDataController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<FakeDataController> _logger;

        public FakeDataController(ILogger<FakeDataController> logger)
        {
            _logger = logger;
        }

        [HttpGet("random-number")]
        public Task<int> GetRandomNumber()
        {
            var rng = new Random();

            return Task.FromResult(rng.Next(1, 10));
        }
    }
}
