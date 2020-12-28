using System;
using System.Collections.Generic;
using System.Linq;
using IFire.Auth.Abstractions;
using IFire.Framework.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;

namespace IFire.WebHost.Controllers {

    [Route("api/v{version:apiVersion}/test")]
    public class WeatherForecastController : ControllerAbstract {

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfigProvider _configProvider;
        private readonly IHttpContextAccessor _accessor;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfigProvider configProvider, IHttpContextAccessor accessor) {
            _logger = logger;
            _configProvider = configProvider;
            _accessor = accessor;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get() {
            var aa = _configProvider.Get<AuthConfig>("Auth");
            var htmlString = MiniProfiler.Current.RenderIncludes(_accessor.HttpContext);
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
