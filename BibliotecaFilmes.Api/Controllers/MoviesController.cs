using System.ComponentModel.DataAnnotations;
using BibliotecaFilmes.Api.DTOs;
using BibliotecaFilmes.Api.Models;
using BibliotecaFilmes.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaFilmes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class MoviesController(IInMemoryMovieRepository repository) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<MovieResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<MovieResponse> Create(CreateMovieRequest request)
    {
        var movie = repository.Add(new Movie
        {
            Title = request.Title,
            Director = request.Director,
            ReleaseYear = request.ReleaseYear,
            Genre = request.Genre,
            Rating = request.Rating
        });

        var response = ToResponse(movie);
        return CreatedAtAction(nameof(GetById), new { id = movie.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType<PagedMoviesResponse>(StatusCodes.Status200OK)]
    public ActionResult<PagedMoviesResponse> GetAll(
        [FromQuery] string? title,
        [FromQuery] string? genre,
        [FromQuery, Range(1, int.MaxValue)] int page = 1,
        [FromQuery, Range(1, 100)] int pageSize = 10)
    {
        var movies = repository.GetAll().AsEnumerable();

        if (!string.IsNullOrWhiteSpace(title))
        {
            movies = movies.Where(movie =>
                movie.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(genre))
        {
            movies = movies.Where(movie =>
                string.Equals(movie.Genre, genre, StringComparison.OrdinalIgnoreCase));
        }

        var filteredMovies = movies.OrderBy(movie => movie.Title).ToArray();
        var skip = (int)Math.Min((long)(page - 1) * pageSize, int.MaxValue);
        var response = new PagedMoviesResponse
        {
            Items = filteredMovies
                .Skip(skip)
                .Take(pageSize)
                .Select(ToResponse)
                .ToArray(),
            TotalCount = filteredMovies.Length,
            Page = page,
            PageSize = pageSize
        };

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<MovieResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<MovieResponse> GetById(Guid id)
    {
        var movie = repository.GetById(id);
        return movie is null ? NotFound() : Ok(ToResponse(movie));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<MovieResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<MovieResponse> Update(Guid id, UpdateMovieRequest request)
    {
        var movie = new Movie
        {
            Id = id,
            Title = request.Title,
            Director = request.Director,
            ReleaseYear = request.ReleaseYear,
            Genre = request.Genre,
            Rating = request.Rating
        };

        if (!repository.Update(movie))
        {
            return NotFound();
        }

        var updatedMovie = repository.GetById(id);
        return updatedMovie is null ? NotFound() : Ok(ToResponse(updatedMovie));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id) =>
        repository.Delete(id) ? NoContent() : NotFound();

    private static MovieResponse ToResponse(Movie movie) => new()
    {
        Id = movie.Id,
        Title = movie.Title,
        Director = movie.Director,
        ReleaseYear = movie.ReleaseYear,
        Genre = movie.Genre,
        Rating = movie.Rating,
        CreatedAt = movie.CreatedAt,
        UpdatedAt = movie.UpdatedAt
    };
}
