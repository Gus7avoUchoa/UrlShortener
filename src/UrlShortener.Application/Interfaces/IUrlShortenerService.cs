using UrlShortener.Application.DTOs;

namespace UrlShortener.Application.Interfaces;

public interface IUrlShortenerService
{
    Task<ShortenUrlResult> ShortenUrlAsync(string originalUrl, string alias);
    Task<string> RetrieveUrlAsync(string alias);
}