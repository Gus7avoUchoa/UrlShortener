using UrlShortener.Application.DTOs;

namespace UrlShortener.Application.Interfaces;

public interface IUrlShortenerService
{
    Task<ShortenUrlResult> ShortenUrlAsync(string originalUrl, string customAlias);
    Task<string> RetrieveUrlAsync(string shortUrl);
}