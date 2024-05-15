using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Core.Entities;

public class UrlEntry
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string OriginalUrl { get; set; }

    [Required]
    public required string Alias { get; set; }

    [Required]
    [StringLength(50)]
    public required string ShortUrl { get; set; }

    public DateTime CreatedAt { get; set; }
}