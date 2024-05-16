using UrlShortener.Core.Interfaces;

namespace UrlShortener.Core.Utilities;

public class GenerateUniqueAlias(IGenerateRandomString generateRandomString, IUrlRepository urlRepository) : IGenerateUniqueAlias
{
    private readonly IGenerateRandomString _generateRandomString = generateRandomString;
    private readonly IUrlRepository _urlRepository = urlRepository;
    
    public async Task<string> Generate()
    {
        string alias;
        do
        {
            alias = await _generateRandomString.Generate(6);
        } while (await _urlRepository.AliasExistsAsync(alias));
        return alias;
    }
}