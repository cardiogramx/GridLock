using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;

using GridLock.AspNetCore.Mvc.Authorization;
using GridLock;

namespace aspnetcore3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IGridLock gridLock;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IGridLock gridLock)
        {
            _logger = logger;
            this.gridLock = gridLock;
        }


        [HttpGet("Add/{id}/{level}")]
        public async Task<GridLockItem> Add(string Id, int? level, CancellationToken cancellation)
        {
            return level switch
            {
                null => await gridLock.AddAsync(new Client { Id = Id, Level = 1 }),
                _ => await gridLock.AddAsync(new Client { Id = Id, Level = level.Value }, cancellation)
            };
        }

        [HttpGet("Test")]
        [Locked(AuthorizedLevels = new int[]{ 1 })]
        public IEnumerable<WeatherForecast> Test()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet("First")]
        public async Task<Client> First(CancellationToken cancellation)
        {
            var response = await gridLock.ListAsync(cancellation);

            return response.First().ToObject<Client>();
        }


        [HttpGet("List")]
        public async Task<List<GridLockItem>> List(CancellationToken cancellation)
        {
            var response = await gridLock.ListAsync(cancellation);

            return response;
        }


        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }


    }
}
