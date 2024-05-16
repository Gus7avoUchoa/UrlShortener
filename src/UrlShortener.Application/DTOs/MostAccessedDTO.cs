namespace UrlShortener.Application.DTOs;

public class MostAccessedDTO
{
    public int AccessCount { get; set; }
    public required string OriginalUrl { get; set; }    
}