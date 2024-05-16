using FluentAssertions;
using Moq;
using UrlShortener.Core.Interfaces;
using UrlShortener.Core.Utilities;

namespace UrlShortener.Tests.Utilities;

public class GenerateUniqueAliasTests
{
    private readonly Mock<IGenerateRandomString> _mockGenerateRandomString;
    private readonly Mock<IUrlRepository> _mockUrlRepository;
    private readonly GenerateUniqueAlias _generateUniqueAlias;

    public GenerateUniqueAliasTests()
    {
        _mockGenerateRandomString = new Mock<IGenerateRandomString>();
        _mockUrlRepository = new Mock<IUrlRepository>();
        _generateUniqueAlias = new GenerateUniqueAlias(_mockGenerateRandomString.Object, _mockUrlRepository.Object);
    }

    [Fact]
    public async Task Generate_ShouldReturnUniqueAlias_WhenAliasDoesNotExist()
    {
        // Arrange
        var alias = "abc123";
        _mockGenerateRandomString.Setup(x => x.Generate(It.IsAny<int>())).Returns(Task.FromResult(alias));
        _mockUrlRepository.Setup(x => x.AliasExistsAsync(alias)).ReturnsAsync(false);

        // Act
        var result = await _generateUniqueAlias.Generate();

        // Assert
        result.Should().Be(alias);
        _mockGenerateRandomString.Verify(x => x.Generate(It.IsAny<int>()), Times.Once);
        _mockUrlRepository.Verify(x => x.AliasExistsAsync(alias), Times.Once);
    }

    [Fact]
    public async Task Generate_ShouldCallGenerateAndAliasExistsAsync()
    {
        // Arrange
        var alias = "abc123";
        _mockGenerateRandomString.Setup(x => x.Generate(It.IsAny<int>())).Returns(Task.FromResult(alias));
        _mockUrlRepository.SetupSequence(x => x.AliasExistsAsync(alias))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        // Act
        var result = await _generateUniqueAlias.Generate();

        // Assert
        result.Should().Be(alias);
        _mockGenerateRandomString.Verify(x => x.Generate(It.IsAny<int>()), Times.Exactly(2));
        _mockUrlRepository.Verify(x => x.AliasExistsAsync(alias), Times.Exactly(2));
    }
}