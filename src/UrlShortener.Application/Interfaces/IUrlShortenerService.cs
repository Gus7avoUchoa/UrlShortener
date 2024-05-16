using UrlShortener.Application.DTOs;

namespace UrlShortener.Application.Interfaces;

public interface IUrlShortenerService
{
    Task<ShortenUrlResultDTO> ShortenUrlAsync(string originalUrl, string customAlias);
    Task<string> RetrieveUrlAsync(string shortUrl);
    Task<List<MostAccessedDTO>> GetMostAccessedUrlsAsync();
}