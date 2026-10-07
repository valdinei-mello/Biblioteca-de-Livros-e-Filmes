namespace BibliotecaFilmes.Api.DTOs;

/// <summary>Resultado paginado da consulta de filmes.</summary>
public sealed class PagedMoviesResponse
{
    /// <summary>Filmes da página atual.</summary>
    public IReadOnlyCollection<MovieResponse> Items { get; init; } = [];

    /// <summary>Total de filmes que correspondem aos filtros.</summary>
    public int TotalCount { get; init; }

    /// <summary>Número da página atual.</summary>
    public int Page { get; init; }

    /// <summary>Quantidade máxima de filmes por página.</summary>
    public int PageSize { get; init; }
}
