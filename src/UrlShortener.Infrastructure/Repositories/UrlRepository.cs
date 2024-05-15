using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;
using UrlShortener.Infrastructure.Data;

namespace UrlShortener.Infrastructure.Repositories;

public class UrlRepository(UrlShortenerContext context) : IUrlRepository
{
    private readonly UrlShortenerContext _context = context;

    public async Task<UrlEntry> GetByAliasAsync(string alias)
    {
        return await _context.UrlEntries.FirstOrDefaultAsync(x => x.Alias == alias) ?? throw new Exception("Alias não encontrado.");
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
}