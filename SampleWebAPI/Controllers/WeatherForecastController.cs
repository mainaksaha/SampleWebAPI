using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("forecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogError("forecast trace");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
            
        }

        [HttpGet("errortest")]
        public IEnumerable<WeatherForecast> GetError()
        {
            int j = 0;
            try
            {
                int k = 10 / j;
            }
            catch (Exception ex)
            {
                _logger.LogError("errortest trace");
                _logger.LogError(ex, ex.Message);
            }
            _logger.LogError("Weather is not good here");
            var rng = new Random();
            _logger.LogError("trace after handled exception");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("faulttest")]
        public IEnumerable<WeatherForecast> GetFault()
        {
            _logger.LogError("faulttest message");
            int j = 0;
            int k = 10 / j;
            var rng = new Random();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("api-identifier")]
        public string Identify()
        {
            return "Adaptive Sampling - v1.0";
        }
    }
}
