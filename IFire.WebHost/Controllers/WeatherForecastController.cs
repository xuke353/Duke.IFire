using System;
using System.Collections.Generic;
using System.Linq;
using IFire.Auth.Abstractions;
using IFire.Auth.Web;
using IFire.Framework.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IFire.WebHost.Controllers {

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/sfsdf")]
    public class WeatherForecastController : ControllerAbstract {

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfigProvider _configProvider;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfigProvider configProvider) {
            _logger = logger;
            _configProvider = configProvider;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get() {
            var aa = _configProvider.Get<AuthConfig>("Auth");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
