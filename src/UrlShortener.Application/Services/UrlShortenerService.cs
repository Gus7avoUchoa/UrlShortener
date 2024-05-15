using UrlShortener.Application.DTOs;
using UrlShortener.Application.Interfaces;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.Application.Services;

public class UrlShortenerService(IUrlRepository urlRepository) : IUrlShortenerService
{
    private readonly IUrlRepository _urlRepository = urlRepository;

    public async Task<ShortenUrlResult> ShortenUrlAsync(string originalUrl, string customAlias)
    {
        if (!string.IsNullOrEmpty(customAlias) && await _urlRepository.AliasExistsAsync(customAlias))
        {
            return new ShortenUrlResult
            {
                ErrorCode = "001",
                Description = "CUSTOM ALIAS ALREADY EXISTS"
            };
        }

        var alias = string.IsNullOrEmpty(customAlias) ? await GenerateUniqueAliasAsync() : customAlias;
        var urlEntry = new UrlEntry
        {
            OriginalUrl = originalUrl,
            Alias = alias,
            ShortUrl = $"http://urlshortener/u/{alias}",
            CreatedAt = DateTime.Now
        };
        await _urlRepository.AddAsync(urlEntry);

        return new ShortenUrlResult
        {
            Alias = alias,
            ShortUrl = urlEntry.ShortUrl,
            Statistics = new { time_taken = "Calcular tempo de execução" }
        };
    }

    public async Task<string> RetrieveUrlAsync(string alias)
    {
        var urlEntry = await _urlRepository.GetByAliasAsync(alias);
        return urlEntry.OriginalUrl;
    }
    
    private async Task<string> GenerateUniqueAliasAsync()
    {
        const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        
        // string alias = new(Enumerable.Repeat(Alphabet, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        // return await _urlRepository.AliasExistsAsync(alias) ? await GenerateUniqueAliasAsync() : alias;

        string alias;
        do
        {
            alias = new(Enumerable.Repeat(Alphabet, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        } while (await _urlRepository.AliasExistsAsync(alias));

        return alias;
    }
}