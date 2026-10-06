namespace BibliotecaFilmes.Api.Models;

public sealed record Movie
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Director { get; init; } = string.Empty;
    public int ReleaseYear { get; init; }
    public string Genre { get; init; } = string.Empty;
    public decimal Rating { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}
