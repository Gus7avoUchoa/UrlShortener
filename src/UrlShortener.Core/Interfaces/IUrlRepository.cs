using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Interfaces;

public interface IUrlRepository
{
    Task<UrlEntry> GetByAliasAsync(string alias);
    Task AddAsync(UrlEntry urlEntry);
    Task<bool> AliasExistsAsync(string alias);
    // Task DeleteAsync(int id);
}