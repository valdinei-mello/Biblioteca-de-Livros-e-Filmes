using System.Net;
using System.Net.Http.Json;
using BibliotecaFilmes.Api.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BibliotecaFilmes.Tests;

public class MoviesApiTests
{
    [Fact]
    public async Task MoviesEndpoints_ValidateRatingAndSupportCrudAndPagination()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var invalidMovie = new
        {
            Title = "Arrival",
            Director = "Denis Villeneuve",
            ReleaseYear = 2016,
            Genre = "Sci-Fi",
            Rating = 6
        };
        var invalidResponse = await client.PostAsJsonAsync("/api/movies", invalidMovie);

        invalidResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var validMovie = new
        {
            Title = "Arrival",
            Director = "Denis Villeneuve",
            ReleaseYear = 2016,
            Genre = "Sci-Fi",
            Rating = 5
        };
        var createResponse = await client.PostAsJsonAsync("/api/movies", validMovie);

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdMovie = await createResponse.Content.ReadFromJsonAsync<MovieResponse>()
            ?? throw new InvalidOperationException("A resposta de criação não contém o filme.");

        var getResponse = await client.GetAsync($"/api/movies/{createdMovie.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var listResponse = await client.GetFromJsonAsync<PagedMoviesResponse>(
            "/api/movies?genre=sci-fi&page=1&pageSize=1")
            ?? throw new InvalidOperationException("A resposta da listagem está vazia.");
        listResponse.TotalCount.Should().Be(1);
        listResponse.Items.Should().ContainSingle()
            .Which.Id.Should().Be(createdMovie.Id);

        var updatedMovie = new
        {
            Title = "Arrival Updated",
            Director = "Denis Villeneuve",
            ReleaseYear = 2016,
            Genre = "Sci-Fi",
            Rating = 4
        };
        var updateResponse = await client.PutAsJsonAsync(
            $"/api/movies/{createdMovie.Id}", updatedMovie);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var deleteResponse = await client.DeleteAsync($"/api/movies/{createdMovie.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var getDeletedMovie = await client.GetAsync($"/api/movies/{createdMovie.Id}");
        getDeletedMovie.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
