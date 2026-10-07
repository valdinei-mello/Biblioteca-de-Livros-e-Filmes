using System.ComponentModel.DataAnnotations;

namespace BibliotecaFilmes.Api.DTOs;

/// <summary>Dados necessários para cadastrar um filme favorito.</summary>
public sealed class CreateMovieRequest
{
    /// <summary>Título do filme.</summary>
    [Required]
    public string Title { get; init; } = string.Empty;

    /// <summary>Nome do diretor.</summary>
    [Required]
    public string Director { get; init; } = string.Empty;

    /// <summary>Ano de lançamento.</summary>
    public int ReleaseYear { get; init; }

    /// <summary>Gênero do filme.</summary>
    [Required]
    public string Genre { get; init; } = string.Empty;

    /// <summary>Avaliação entre 1 e 5.</summary>
    [Range(typeof(decimal), "1", "5")]
    public decimal Rating { get; init; }
}
