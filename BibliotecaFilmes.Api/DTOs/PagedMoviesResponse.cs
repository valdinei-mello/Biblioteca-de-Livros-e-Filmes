namespace BibliotecaFilmes.Api.DTOs;

public sealed class PagedMoviesResponse
{
    public IReadOnlyCollection<MovieResponse> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}
