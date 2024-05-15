using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Models;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlShortenerController(IUrlShortenerService urlShortenerService) : ControllerBase
{
    private readonly IUrlShortenerService _urlShortenerService = urlShortenerService;

    [HttpPut("create")]
    public async Task<IActionResult> CreateShortUrlAsync([FromBody] UrlRequestModel request)
    {
        var stopwatch = Stopwatch.StartNew();

        var result = await _urlShortenerService.ShortenUrlAsync(request.Url, request.CustomAlias);
        if (result.HasError)
            return Conflict(new
            {
                alias = request.CustomAlias,
                err_code = "001",
                description = "CUSTOM ALIAS ALREADY EXISTS"
            });

        stopwatch.Stop();

        return Ok(new
        {
            alias = result.Alias,
            url = result.ShortUrl,
            statistics = new
            {
                time_taken = $"{stopwatch.ElapsedMilliseconds}ms"
            }
        });
    }

    [HttpGet]
    public async Task<IActionResult> RetrieveUrlAsync([FromQuery] string shortUrl)
    {
        var originalUrl = await _urlShortenerService.RetrieveUrlAsync(shortUrl);
        return string.IsNullOrWhiteSpace(originalUrl) ?
            NotFound(new
            {
                err_code = "002",
                Description = "SHORTENED URL NOT FOUND"
            }) : Ok(originalUrl);
    }
}