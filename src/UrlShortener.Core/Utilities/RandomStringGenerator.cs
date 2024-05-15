using System.Text;

namespace UrlShortener.Core.Utilities;

public static class RandomStringGenerator
{
    private static readonly Random _random = new();
    private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string Generate(int length)
    {
        var result = new StringBuilder(length);
        lock (_random)
        {
            for (var i = 0; i < length; i++)
            {
                result.Append(_chars[_random.Next(_chars.Length)]);
            }
        }
        return result.ToString();
    }
}