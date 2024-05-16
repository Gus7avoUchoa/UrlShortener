using FluentAssertions;
using Moq;
using UrlShortener.Application.Services;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.Tests.Services;

public class UrlShortenerServiceTests
{
    private readonly Mock<IUrlRepository> _mockRepository;
    private readonly Mock<IGenerateRandomString> _mockGenerateRandomString;
    private readonly UrlShortenerService _service;

    public UrlShortenerServiceTests()
    {
        _mockRepository = new Mock<IUrlRepository>();
        _mockGenerateRandomString = new Mock<IGenerateRandomString>();
        _service = new UrlShortenerService(_mockRepository.Object, _mockGenerateRandomString.Object);
    }

    [Fact]
    public async Task ShortenUrlAsync_WithValidUrlAndNoCustomAlias_ReturnsShortenedUrl()
    {
        // Arrange
        var originalUrl = "https://www.google.com";
        var generatedAlias = "abc123";
        _mockGenerateRandomString.Setup(g => g.Generate(It.IsAny<int>())).Returns(Task.FromResult(generatedAlias));
        _mockRepository.Setup(r => r.AliasExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

        // Act
        var result = await _service.ShortenUrlAsync(originalUrl, string.Empty);

        // Assert
        result.Should().NotBeNull();
        result.ShortUrl.Should().Be($"http://urlshortener/u/{generatedAlias}");
        result.HasError.Should().BeFalse();
    }

    [Fact]
    public async Task ShortenUrlAsync_WithCustomAlias_ReturnsShortenedUrl()
    {
        // Arrange
        var originalUrl = "https://www.google.com";
        var customAlias = "google";
        _mockRepository.Setup(x => x.AliasExistsAsync(customAlias)).ReturnsAsync(false);

        // Act
        var result = await _service.ShortenUrlAsync(originalUrl, customAlias);

        // Assert
        result.Should().NotBeNull();
        result.HasError.Should().BeFalse();
        result.ShortUrl.Should().Be($"http://urlshortener/u/{customAlias}");
    }

    [Fact]
    public async Task ShortenUrlAsync_WithExistingCustomAlias_ReturnsError()
    {
        // Arrange
        var originalUrl = "https://www.google.com";
        var customAlias = "google";
        _mockRepository.Setup(x => x.AliasExistsAsync(customAlias)).ReturnsAsync(true);

        // Act
        var result = await _service.ShortenUrlAsync(originalUrl, customAlias);

        // Assert
        result.Should().NotBeNull();
        result.HasError.Should().BeTrue();
    }

    [Fact]
    public async Task RetrieveUrlAsync_WithExistingAlias_ReturnsOriginalUrl()
    {
        // Arrange
        var shortUrl = "http://urlshortener/u/google";
        var urlEntry = new UrlEntry { OriginalUrl = "https://www.google.com" };
        _mockRepository.Setup(x => x.GetByShortUrlAsync(shortUrl)).ReturnsAsync(urlEntry);

        // Act
        var result = await _service.RetrieveUrlAsync(shortUrl);

        // Assert
        result.Should().Be(urlEntry.OriginalUrl);
    }

    [Fact]
    public async Task RetrieveUrlAsync_WithNonExistingAlias_ReturnsEmptyString()
    {
        // Arrange
        var shortUrl = "http://urlshortener/u/nonexistent";
        _mockRepository.Setup(x => x.GetByShortUrlAsync(shortUrl)).ReturnsAsync((UrlEntry)null);

        // Act
        var result = await _service.RetrieveUrlAsync(shortUrl);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetMostAccessedUrlsAsync_ReturnsMostAccessedUrls()
    {
        // Arrange
        var mostAccessedUrls = new List<(int AccessCount, string OriginalUrl)>
        {
            (10, "https://www.google.com/"),
            (5, "https://www.microsoft.com/")
        };
        _mockRepository.Setup(x => x.GetMostAccessedUrlsAsync()).ReturnsAsync(mostAccessedUrls);

        // Act
        var result = await _service.GetMostAccessedUrlsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(x => x.AccessCount > 5 && x.OriginalUrl == "https://www.google.com/");        
    }
}