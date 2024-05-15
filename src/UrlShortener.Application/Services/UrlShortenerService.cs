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
            return new ShortenUrlResult{ HasError = true };

        var alias = string.IsNullOrEmpty(customAlias) ? await GenerateUniqueAliasAsync() : customAlias;
        
        // TODO: Criar um HASH para a URL
        var urlEntry = new UrlEntry
        {
            OriginalUrl = originalUrl,
            Alias = alias,
            ShortUrl = $"http://urlshortener/u/{alias}",
            CreatedAt = DateTime.Now
        };
        await _urlRepository.AddAsync(urlEntry);
    
        // TODO: Calcular tempo de execução
        return new ShortenUrlResult
        {
            Alias = alias,
            ShortUrl = urlEntry.ShortUrl,
            Statistics = new { time_taken = "Calcular tempo de execução" }
        };
    }

    public async Task<string> RetrieveUrlAsync(string shortUrl)
    {
        return await _urlRepository.GetByShortUrlAsync(shortUrl) is { } urlEntry ? urlEntry.OriginalUrl : string.Empty;
    }
    
    private async Task<string> GenerateUniqueAliasAsync()
    {
        const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        var random = new Random();        
        string alias = new(Enumerable.Repeat(Alphabet, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        return await _urlRepository.AliasExistsAsync(alias) ? await GenerateUniqueAliasAsync() : alias;
    }
}