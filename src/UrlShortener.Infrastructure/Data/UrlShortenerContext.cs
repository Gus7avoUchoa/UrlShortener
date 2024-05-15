using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Entities;

namespace UrlShortener.Infrastructure.Data;

public class UrlShortenerContext(DbContextOptions<UrlShortenerContext> options) : DbContext(options)
{
    public DbSet<UrlEntry> UrlEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
