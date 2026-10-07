namespace BibliotecaFilmes.Api.DTOs;

/// <summary>Dados de um filme retornados pela API.</summary>
public sealed class MovieResponse
{
    /// <summary>Identificador único do filme.</summary>
    public Guid Id { get; init; }

    /// <summary>Título do filme.</summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>Nome do diretor.</summary>
    public string Director { get; init; } = string.Empty;

    /// <summary>Ano de lançamento.</summary>
    public int ReleaseYear { get; init; }

    /// <summary>Gênero do filme.</summary>
    public string Genre { get; init; } = string.Empty;

    /// <summary>Avaliação entre 1 e 5.</summary>
    public decimal Rating { get; init; }

    /// <summary>Data e hora de criação em UTC.</summary>
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>Data e hora da última atualização em UTC.</summary>
    public DateTimeOffset UpdatedAt { get; init; }
}
