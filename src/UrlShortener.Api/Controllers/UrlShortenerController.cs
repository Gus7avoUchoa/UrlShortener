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
        var result = await _urlShortenerService.ShortenUrlAsync(request.OriginalUrl, request.CustomAlias);
        if (result.ErrorCode != null && result.ErrorCode == "001")
            return Conflict( new { err_code = result.ErrorCode, alias = result.Description } );
        
        return Ok(result);
    }

    [HttpGet("{alias}")]
    public async Task<IActionResult> RetrieveUrlAsync(string alias)
    {
        var originalUrl = await _urlShortenerService.RetrieveUrlAsync(alias);
        if (originalUrl == null)
            return NotFound(new { err_code = "002", alias = "ALIAS NOT FOUND" });

        return Redirect(originalUrl);
    }
}