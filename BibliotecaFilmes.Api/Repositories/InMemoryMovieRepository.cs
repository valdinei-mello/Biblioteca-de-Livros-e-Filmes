using System.Collections.Concurrent;
using BibliotecaFilmes.Api.Models;

namespace BibliotecaFilmes.Api.Repositories;

public sealed class InMemoryMovieRepository : IInMemoryMovieRepository
{
    private readonly ConcurrentDictionary<Guid, Movie> _movies = new();

    public IReadOnlyCollection<Movie> GetAll() => _movies.Values.ToArray();

    public Movie? GetById(Guid id) =>
        _movies.TryGetValue(id, out var movie) ? movie : null;

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

    public bool Delete(Guid id) => _movies.TryRemove(id, out _);
}
