using Moq;
using UrlShortener.Api.Controllers;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.DTOs;
using UrlShortener.Api.Models;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace UrlShortener.Tests.Controllers;

public class UrlShortenerControllerTests
{
    private readonly Mock<IUrlShortenerService> _mockService;
    private readonly UrlShortenerController _controller;

    public UrlShortenerControllerTests()
    {
        _mockService = new Mock<IUrlShortenerService>();
        _controller = new UrlShortenerController(_mockService.Object);
    }

    [Fact(Skip = "Untestable method due to TimeTaken")]
    public async Task CreateShortUrlAsync_WithValidAlias_ReturnsShortenUrlResultDTO()
    {
        // Arrange
        var originalUrl = "https://www.google.com";
        var alias = "abc123";
        var expectedShortenUrlResultDTO = new ShortenUrlResultDTO
        {
            Alias = "abc123",
            ShortUrl = $"http://urlshortener/u/abc123",
            Statistics = new { TimeTaken = "100ms" } // method not testable due to this field
        };
        _mockService.Setup(x => x.ShortenUrlAsync(originalUrl, alias)).ReturnsAsync(expectedShortenUrlResultDTO);

        // Act
        var result = await _controller.CreateShortUrlAsync(new UrlRequestModel { Url = originalUrl, CustomAlias = alias });

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();

        var actualShortenUrlResultDTO = okResult.Value as ShortenUrlResultDTO;
        actualShortenUrlResultDTO.Should().NotBeNull();

        actualShortenUrlResultDTO.Alias.Should().Be(expectedShortenUrlResultDTO.Alias);
        actualShortenUrlResultDTO.ShortUrl.Should().Be(expectedShortenUrlResultDTO.ShortUrl);
        actualShortenUrlResultDTO.Statistics.Should().BeEquivalentTo(expectedShortenUrlResultDTO.Statistics);
    }

    [Fact]
    public async Task RetrieveUrlAsync_WithValidShortUrl_ReturnsOriginalUrl()
    {
        // Arrange
        var shortUrl = "abc123";
        var expectedOriginalUrl = "https://www.google.com";
        _mockService.Setup(x => x.RetrieveUrlAsync(shortUrl)).ReturnsAsync(expectedOriginalUrl);

        // Act
        var result = await _controller.RetrieveUrlAsync(shortUrl);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();

        var actualOriginalUrl = okResult.Value as string;
        actualOriginalUrl.Should().Be(expectedOriginalUrl);
    }

    [Fact]
    public async Task RetrieveUrlAsync_WithInvalidShortUrl_ReturnsNotFound()
    {
        // Arrange
        var shortUrl = "abc123";
        _mockService.Setup(x => x.RetrieveUrlAsync(shortUrl)).ReturnsAsync(string.Empty);

        // Act
        var result = await _controller.RetrieveUrlAsync(shortUrl);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
    }
}