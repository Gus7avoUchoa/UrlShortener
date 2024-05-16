namespace UrlShortener.Core.Interfaces;

public interface IGenerateUniqueAlias
{
    Task<string> Generate();
}