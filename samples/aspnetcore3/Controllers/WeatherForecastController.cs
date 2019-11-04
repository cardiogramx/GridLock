using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;

using GridLock.AspNetCore.Mvc.Authorization;
using GridLock;
using Newtonsoft.Json;

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

            //GridLock custom configuration
            gridLock.Options = new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
            {
                //AbsoluteExpiration = DateTime.UtcNow.AddDays(1),
                SlidingExpiration = TimeSpan.FromHours(1)
            };

            this.gridLock = gridLock;

            this.gridLock.OnComitting += GridLock_OnComitting;
        }

        private void GridLock_OnComitting(object sender, GridLockEventArgs e)
        {
            var client = e.Item as Client;
            _logger.LogInformation(JsonConvert.SerializeObject(client));

            //do useful work
        }

        [HttpGet("Add/{id}/{level}")]
        public async Task<Client> Add(string Id, int? level, CancellationToken cancellation)
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
        public async Task<string> First(CancellationToken cancellation)
        {
            var response = await gridLock.ListAsync<Client>(cancellation);

            return response.First().DateTimeAdded.ToString();
        }


        [HttpGet("List")]
        public async Task<List<GridLockItem>> List(CancellationToken cancellation)
        {
            var response = await gridLock.ListAsync<GridLockItem>(cancellation);

            return response;
        }

        [HttpGet("List2")]
        public async Task<List<Client>> List2(CancellationToken cancellation)
        {
            var response = await gridLock.ListAsync<Client>(cancellation);

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
