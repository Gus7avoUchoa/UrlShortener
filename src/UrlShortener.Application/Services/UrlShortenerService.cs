using UrlShortener.Application.DTOs;
using UrlShortener.Application.Interfaces;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;
using UrlShortener.Core.Utilities;

namespace UrlShortener.Application.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly IUrlRepository _urlRepository;
    private readonly IGenerateUniqueAlias _generateUniqueAlias;

    public UrlShortenerService(IUrlRepository urlRepository, IGenerateRandomString generateRandomString)
    {
        _urlRepository = urlRepository;
        _generateUniqueAlias = new GenerateUniqueAlias(generateRandomString, _urlRepository);
    }

    public async Task<ShortenUrlResultDTO> ShortenUrlAsync(string originalUrl, string customAlias)
    {
        if (!string.IsNullOrEmpty(customAlias) && await _urlRepository.AliasExistsAsync(customAlias))
            return new ShortenUrlResultDTO { HasError = true };

        var alias = string.IsNullOrEmpty(customAlias) ? await _generateUniqueAlias.Generate() : customAlias;
        var urlEntry = new UrlEntry
        {
            OriginalUrl = originalUrl,
            Alias = alias,
            ShortUrl = $"http://urlshortener/u/{alias}",
            CreatedAt = DateTime.Now
        };
        await _urlRepository.AddAsync(urlEntry);

        return new ShortenUrlResultDTO
        {
            Alias = alias,
            ShortUrl = urlEntry.ShortUrl
        };
    }

    public async Task<string> RetrieveUrlAsync(string shortUrl)
    {
        return await _urlRepository.GetByShortUrlAsync(shortUrl) is { } urlEntry ? urlEntry.OriginalUrl : string.Empty;
    }

    public async Task<List<MostAccessedDTO>> GetMostAccessedUrlsAsync()
    {
        var mostAccessedUrls = await _urlRepository.GetMostAccessedUrlsAsync();
        return mostAccessedUrls.Select(x => new MostAccessedDTO
        {
            AccessCount = x.AccessCount,
            OriginalUrl = x.OriginalUrl
        }).ToList();
    }
}