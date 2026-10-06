using System.ComponentModel.DataAnnotations;

namespace BibliotecaFilmes.Api.DTOs;

public sealed class UpdateMovieRequest
{
    [Required]
    public string Title { get; init; } = string.Empty;

    [Required]
    public string Director { get; init; } = string.Empty;

    public int ReleaseYear { get; init; }

    [Required]
    public string Genre { get; init; } = string.Empty;

    [Range(typeof(decimal), "1", "5")]
    public decimal Rating { get; init; }
}
