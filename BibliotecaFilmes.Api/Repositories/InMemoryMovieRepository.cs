using System.Collections.Concurrent;
using BibliotecaFilmes.Api.Models;

namespace BibliotecaFilmes.Api.Repositories;

/// <summary>Armazena filmes em um dicionário concorrente na memória.</summary>
public sealed class InMemoryMovieRepository : IInMemoryMovieRepository
{
    private readonly ConcurrentDictionary<Guid, Movie> _movies = new();

    /// <inheritdoc />
    public IReadOnlyCollection<Movie> GetAll() => _movies.Values.ToArray();

    /// <inheritdoc />
    public Movie? GetById(Guid id) =>
        _movies.TryGetValue(id, out var movie) ? movie : null;

    /// <inheritdoc />
    public Movie Add(Movie movie)
    {
        var now = DateTimeOffset.UtcNow;
        var newMovie = movie with
        {
            Id = Guid.NewGuid(),
            CreatedAt = now,
            UpdatedAt = now
        };

        while (!_movies.TryAdd(newMovie.Id, newMovie))
        {
            newMovie = newMovie with { Id = Guid.NewGuid() };
        }

        return newMovie;
    }

    /// <inheritdoc />
    public bool Update(Movie movie)
    {
        while (_movies.TryGetValue(movie.Id, out var existing))
        {
            var updatedMovie = movie with
            {
                CreatedAt = existing.CreatedAt,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            if (_movies.TryUpdate(movie.Id, updatedMovie, existing))
            {
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc />
    public bool Delete(Guid id) => _movies.TryRemove(id, out _);
}
