using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Api.Models;

public class UrlRequestModel
{
    [Required(ErrorMessage = "{0} é obrigatório")]
    [Url(ErrorMessage = "{0} deve ser válida")]
    public required string Url { get; set; }
    public string? CustomAlias { get; set; }
}