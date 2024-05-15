using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Application.DTOs;

public class ShortenUrlResult
{
    public string? Alias { get; set; }

    [Url]
    public string? ShortUrl { get; set; }

    public string? ErrorCode { get; set; }

    public string? Description { get; set; }

    public object? Statistics { get; set; }

    public bool HasError { get; set;}
}