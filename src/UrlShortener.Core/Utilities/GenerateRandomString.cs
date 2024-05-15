using System.Security.Cryptography;

namespace UrlShortener.Core.Utilities;

public class GenerateRandomString : IGenerateRandomString
{
    private static readonly char[] Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    public string Generate(int length)
    {
        var result = new char[length];
        var buffer = new byte[length];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(buffer);
        }

        for (var i = 0; i < length; i++)
        {
            result[i] = Alphabet[buffer[i] % Alphabet.Length];
        }

        return new string(result);
    }
}