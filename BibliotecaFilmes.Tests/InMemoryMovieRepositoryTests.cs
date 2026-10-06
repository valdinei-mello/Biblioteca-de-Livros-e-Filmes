using BibliotecaFilmes.Api.Models;
using BibliotecaFilmes.Api.Repositories;
using FluentAssertions;

namespace BibliotecaFilmes.Tests;

public class InMemoryMovieRepositoryTests
{
    [Fact]
    public void Add_AssignsIdAndTimestamps_AndCanBeRetrieved()
    {
        var repository = new InMemoryMovieRepository();

        var movie = repository.Add(new Movie
        {
            Title = "Arrival",
            Director = "Denis Villeneuve",
            ReleaseYear = 2016,
            Genre = "Sci-Fi",
            Rating = 5
        });

        movie.Id.Should().NotBe(Guid.Empty);
        movie.CreatedAt.Should().Be(movie.UpdatedAt);
        repository.GetById(movie.Id).Should().Be(movie);
        repository.GetAll().Should().ContainSingle();
    }

    [Fact]
    public void Update_PreservesCreatedAt_AndUpdatesMovie()
    {
        var repository = new InMemoryMovieRepository();
        var addedMovie = repository.Add(new Movie { Title = "Arrival" });

        var updatedMovie = addedMovie with { Title = "Updated Arrival" };

        repository.Update(updatedMovie).Should().BeTrue();
        var result = repository.GetById(addedMovie.Id);
        result.Should().NotBeNull();
        result!.CreatedAt.Should().Be(addedMovie.CreatedAt);
        result.Title.Should().Be("Updated Arrival");
    }

    [Fact]
    public void Delete_RemovesMovie()
    {
        var repository = new InMemoryMovieRepository();
        var movie = repository.Add(new Movie { Title = "Arrival" });

        repository.Delete(movie.Id).Should().BeTrue();
        repository.GetById(movie.Id).Should().BeNull();
    }
}
