using System.ComponentModel.DataAnnotations;
using BibliotecaFilmes.Api.DTOs;
using BibliotecaFilmes.Api.Models;
using BibliotecaFilmes.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaFilmes.Api.Controllers;

/// <summary>Expõe operações HTTP para gerenciar filmes favoritos.</summary>
[ApiController]
[Route("api/[controller]")]
public sealed class MoviesController(IInMemoryMovieRepository repository) : ControllerBase
{
    /// <summary>Cadastra um filme favorito.</summary>
    /// <param name="request">Dados do filme a cadastrar.</param>
    /// <returns>O filme criado.</returns>
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

    /// <summary>Lista filmes favoritos com filtros opcionais e paginação.</summary>
    /// <param name="title">Texto opcional para buscar no título.</param>
    /// <param name="genre">Gênero opcional para filtrar.</param>
    /// <param name="page">Número da página, iniciando em 1.</param>
    /// <param name="pageSize">Quantidade de itens por página, entre 1 e 100.</param>
    /// <returns>Filmes correspondentes e metadados da página.</returns>
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

    /// <summary>Busca um filme pelo identificador.</summary>
    /// <param name="id">Identificador do filme.</param>
    /// <returns>O filme encontrado ou HTTP 404.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<MovieResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<MovieResponse> GetById(Guid id)
    {
        var movie = repository.GetById(id);
        return movie is null ? NotFound() : Ok(ToResponse(movie));
    }

    /// <summary>Atualiza os dados de um filme existente.</summary>
    /// <param name="id">Identificador do filme.</param>
    /// <param name="request">Dados atualizados do filme.</param>
    /// <returns>O filme atualizado ou HTTP 404.</returns>
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

    /// <summary>Remove um filme pelo identificador.</summary>
    /// <param name="id">Identificador do filme.</param>
    /// <returns>HTTP 204 se removido ou HTTP 404 se não existir.</returns>
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
