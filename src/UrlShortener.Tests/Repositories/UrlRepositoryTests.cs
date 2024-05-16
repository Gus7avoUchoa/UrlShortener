using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Entities;
using UrlShortener.Infrastructure.Data;
using UrlShortener.Infrastructure.Repositories;

namespace UrlShortener.Tests.Repositories;

public class UrlRepositoryTests
{
    private readonly UrlShortenerContext _context;
    private readonly UrlRepository _repository;

    public UrlRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<UrlShortenerContext>()
            .UseInMemoryDatabase(databaseName: "UrlShortenerTestDb")
            .Options;
        _context = new UrlShortenerContext(options);
        _repository = new UrlRepository(_context);

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        var urlEntries = new List<UrlEntry>
    {
        new() { OriginalUrl = "http://www.vale.com.br/", Alias = "vale", ShortUrl = "http://urlshortener/u/vale" },
        new() { OriginalUrl = "https://gustavouchoa.dev/", Alias = "gustavouchoa", ShortUrl = "http://urlshortener/u/gustavouchoa" },
        new() { OriginalUrl = "https://gustavouchoa.net/", Alias = "gustavo", ShortUrl = "http://urlshortener/u/gustavo" },
        new() { OriginalUrl = "https://www.google.com/", Alias = "aFwt6Y", ShortUrl = "http://urlshortener/u/aFwt6Y" },
        new() { OriginalUrl = "https://www.microsoft.com/", Alias = "j0VOpB", ShortUrl = "http://urlshortener/u/j0VOpB" },
        new() { OriginalUrl = "https://www.microsoft.com/", Alias = "microsoft", ShortUrl = "http://urlshortener/u/microsoft" },
        new() { OriginalUrl = "http://www.vale.com.br/", Alias = "zuWZAK", ShortUrl = "http://urlshortener/u/zuWZAK" },
        new() { OriginalUrl = "http://www.vale.com.br/", Alias = "vale2", ShortUrl = "http://urlshortener/u/vale2" },
        new() { OriginalUrl = "https://mail.google.com/mail/", Alias = "j9sagj", ShortUrl = "http://urlshortener/u/j9sagj" },
        new() { OriginalUrl = "https://mail.google.com/mail/", Alias = "j9sagj1111", ShortUrl = "http://urlshortener/u/j9sagj1111" },
        new() { OriginalUrl = "https://calendar.google.com/", Alias = "Lj0HOE", ShortUrl = "http://urlshortener/u/Lj0HOE" },
        new() { OriginalUrl = "https://calendar.google.com/", Alias = "j3Lb7e", ShortUrl = "http://urlshortener/u/j3Lb7e" },
        new() { OriginalUrl = "https://calendar.google.com/", Alias = "qlfbwy", ShortUrl = "http://urlshortener/u/qlfbwy" },
        new() { OriginalUrl = "https://calendar.google.com/", Alias = "apxebo", ShortUrl = "http://urlshortener/u/apxebo" },
        new() { OriginalUrl = "https://www.google.com/", Alias = "75NCBf", ShortUrl = "http://urlshortener/u/75NCBf" },
        new() { OriginalUrl = "https://www.google.com/", Alias = "Dlrleh", ShortUrl = "http://urlshortener/u/Dlrleh" },
        new() { OriginalUrl = "https://mail.google.com/mail/", Alias = "bZCP8o", ShortUrl = "http://urlshortener/u/bZCP8o" },
        new() { OriginalUrl = "http://www.vale.com.br/", Alias = "gEkItZ", ShortUrl = "http://urlshortener/u/gEkItZ" },
        new() { OriginalUrl = "https://console.cloud.google.com/", Alias = "eRwGca", ShortUrl = "http://urlshortener/u/eRwGca" },
        new() { OriginalUrl = "https://github.com/", Alias = "github", ShortUrl = "http://urlshortener/u/github" },
        new() { OriginalUrl = "https://github.com/", Alias = "ps6i1F", ShortUrl = "http://urlshortener/u/ps6i1F" },
        new() { OriginalUrl = "https://ai.google.dev/", Alias = "ZRFtYF", ShortUrl = "http://urlshortener/u/ZRFtYF" },
        new() { OriginalUrl = "https://ai.google.dev/", Alias = "bbkm4L", ShortUrl = "http://urlshortener/u/bbkm4L" },
    };

        _context.UrlEntries.AddRange(urlEntries);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetByShortUrlAsync_WithExistingShortUrl_ReturnsUrlEntry()
    {
        // Arrange
        var shortUrl = "http://urlshortener/u/vale";

        // Act
        var result = await _repository.GetByShortUrlAsync(shortUrl);

        // Assert
        result.Should().NotBeNull();
        result.OriginalUrl.Should().Be("http://www.vale.com.br/");
    }

    [Fact]
    public async Task GetByShortUrlAsync_WithNonExistingShortUrl_ReturnsNull()
    {
        // Arrange
        var shortUrl = "http://urlshortener/u/xyz123";

        // Act
        var result = await _repository.GetByShortUrlAsync(shortUrl);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WithValidUrlEntry_AddsUrlEntryToDatabase()
    {
        // Arrange
        var urlEntry = new UrlEntry
        {
            OriginalUrl = "https://gustavouchoa.dev/",
            Alias = "gustavouchoa",
            ShortUrl = "http://urlshortener/u/gustavouchoa"
        };

        // Act
        await _repository.AddAsync(urlEntry);
        var result = await _repository.GetByShortUrlAsync(urlEntry.ShortUrl);

        // Assert        
        result.Should().NotBeNull();
        result.OriginalUrl.Should().Be("https://gustavouchoa.dev/");
    }

    [Fact]
    public async Task AliasExistsAsync_WithExistingAlias_ReturnsTrue()
    {
        // Arrange
        var alias = "gustavo";

        // Act
        var result = await _repository.AliasExistsAsync(alias);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AliasExistsAsync_WithNonExistingAlias_ReturnsFalse()
    {
        // Arrange
        var alias = "xyz123";

        // Act
        var result = await _repository.AliasExistsAsync(alias);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetMostAccessedUrlsAsync_ReturnsMostAccessedUrls()
    {
        // Act
        var result = await _repository.GetMostAccessedUrlsAsync();

        // Assert
        result.Should().HaveCount(10);
        result.Should().Contain(x => x.AccessCount > 2 && x.OriginalUrl == "https://www.google.com/");
        result.Should().Contain(x => x.AccessCount == 1 && x.OriginalUrl == "https://gustavouchoa.dev/");
    }

    [Fact]
    public async Task GetMostAccessedUrlsAsync_WithEmptyDatabase_ReturnsEmptyList()
    {
        // Arrange
        _context.UrlEntries.RemoveRange(_context.UrlEntries);
        _context.SaveChanges();

        // Act
        var result = await _repository.GetMostAccessedUrlsAsync();

        // Assert
        result.Should().BeEmpty();
    }
}