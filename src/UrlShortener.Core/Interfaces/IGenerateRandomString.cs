namespace UrlShortener.Core.Interfaces;

public interface IGenerateRandomString
{
    Task<string> Generate(int length);
}