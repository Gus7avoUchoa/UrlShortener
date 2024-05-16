using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;
using UrlShortener.Infrastructure.Data;

namespace UrlShortener.Infrastructure.Repositories;

public class UrlRepository(UrlShortenerContext context) : IUrlRepository
{
    private readonly UrlShortenerContext _context = context;

    public async Task<UrlEntry> GetByShortUrlAsync(string shortUrl)
    {
        return await _context.UrlEntries.FirstOrDefaultAsync(x => x.ShortUrl == shortUrl);
    }

    public async Task AddAsync(UrlEntry urlEntry)
    {
        await _context.UrlEntries.AddAsync(urlEntry);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AliasExistsAsync(string alias)
    {
        return await _context.UrlEntries.AnyAsync(x => x.Alias == alias);
    }

    public async Task<List<(int AccessCount, string OriginalUrl)>> GetMostAccessedUrlsAsync()
    {
        var result = await _context.UrlEntries
                                   .GroupBy(x => x.OriginalUrl)
                                   .OrderByDescending(x => x.Count())
                                   .Take(10)
                                   .Select(x => new { AccessCount = x.Count(), x.Key })
                                   .ToListAsync();

        return result.Select(x => (x.AccessCount, x.Key)).ToList();
    }
}