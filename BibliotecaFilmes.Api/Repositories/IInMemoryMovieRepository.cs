using BibliotecaFilmes.Api.Models;

namespace BibliotecaFilmes.Api.Repositories;

public interface IInMemoryMovieRepository
{
    IReadOnlyCollection<Movie> GetAll();
    Movie? GetById(Guid id);
    Movie Add(Movie movie);
    bool Update(Movie movie);
    bool Delete(Guid id);
}
