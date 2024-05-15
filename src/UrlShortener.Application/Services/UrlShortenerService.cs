using UrlShortener.Application.DTOs;
using UrlShortener.Application.Interfaces;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;
using UrlShortener.Core.Utilities;

namespace UrlShortener.Application.Services;

public class UrlShortenerService(IUrlRepository urlRepository, IGenerateRandomString generateRandomString) : IUrlShortenerService
{
    private readonly IUrlRepository _urlRepository = urlRepository;
    private readonly IGenerateRandomString _generateRandomString = generateRandomString;

    public async Task<ShortenUrlResultDTO> ShortenUrlAsync(string originalUrl, string customAlias)
    {
        if (!string.IsNullOrEmpty(customAlias) && await _urlRepository.AliasExistsAsync(customAlias))
            return new ShortenUrlResultDTO { HasError = true };

        var alias = string.IsNullOrEmpty(customAlias) ? await GenerateUniqueAliasAsync() : customAlias;
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

    private async Task<string> GenerateUniqueAliasAsync()
    {
        string alias;
        do
        {
            alias = _generateRandomString.Generate(6);
        } while (await _urlRepository.AliasExistsAsync(alias));
        return alias;
    }
}