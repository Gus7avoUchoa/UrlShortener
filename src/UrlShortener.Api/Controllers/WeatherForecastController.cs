using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    [HttpGet("GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var rng = new Random();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            rng.Next(-20, 55),
            Summaries[rng.Next(Summaries.Length)]
        ));
    }

    public record WeatherForecast(DateTime Date, int TemperatureC, string Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}