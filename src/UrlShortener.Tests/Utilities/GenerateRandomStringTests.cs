using FluentAssertions;
using UrlShortener.Core.Utilities;

namespace UrlShortener.Tests.Utilities;

public class GenerateRandomStringTests
{
    private readonly GenerateRandomString _generateRandomString;

    public GenerateRandomStringTests()
    {
        _generateRandomString = new GenerateRandomString();
    }

    [Fact]
    public async Task Generate_WithLength_ReturnsStringWithLength()
    {
        // Arrange
        var length = 10;

        // Act
        var result = await _generateRandomString.Generate(length);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Length.Should().Be(length);
    }

    [Fact]
    public async Task Generate_WithValidLength_ReturnsStringWithValidCharacters()
    {
        // Arrange
        var length = 10;
        var validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // Act
        var result = await _generateRandomString.Generate(length);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.All(c => validCharacters.Contains(c)).Should().BeTrue();
    }

    [Fact]
    public void Generate_WithSameLength_ReturnsDifferentStrings()
    {
        // Arrange
        const int length = 10;

        // Act
        var result1 = _generateRandomString.Generate(length);
        var result2 = _generateRandomString.Generate(length);

        // Assert
        result1.Should().NotBe(result2);
    }
}