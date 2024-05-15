namespace UrlShortener.Api.Models;

public class UrlRequestModel
{
    public string OriginalUrl { get; set; }
    public string CustomAlias { get; set; }
}