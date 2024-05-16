using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Interfaces;

public interface IUrlRepository
{    
    Task AddAsync(UrlEntry urlEntry);    
    Task<bool> AliasExistsAsync(string alias);
    Task<UrlEntry> GetByShortUrlAsync(string alias);
    Task<List<(int AccessCount, string OriginalUrl)>> GetMostAccessedUrlsAsync();
}