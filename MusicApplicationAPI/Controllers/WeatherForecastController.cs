using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Services.SongService;

namespace MusicApplicationAPI.Controllers
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

        private ISongService _songService;

        public WeatherForecastController(ISongService songService, ILogger<WeatherForecastController> logger)
        {
            _songService = songService;
            _logger = logger;
        }


        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            await _songService.GetSongsByGenre("Rock");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
