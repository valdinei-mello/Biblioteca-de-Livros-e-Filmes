using BibliotecaFilmes.Api.Models;

namespace BibliotecaFilmes.Api.Repositories;

/// <summary>Define operações de armazenamento em memória para filmes.</summary>
public interface IInMemoryMovieRepository
{
    /// <summary>Retorna todos os filmes armazenados.</summary>
    IReadOnlyCollection<Movie> GetAll();

    /// <summary>Retorna o filme com o identificador informado, se existir.</summary>
    /// <param name="id">Identificador do filme.</param>
    Movie? GetById(Guid id);

    /// <summary>Armazena um filme e atribui identificador e datas.</summary>
    /// <param name="movie">Filme a armazenar.</param>
    /// <returns>Filme armazenado com os campos gerados.</returns>
    Movie Add(Movie movie);

    /// <summary>Atualiza um filme existente.</summary>
    /// <param name="movie">Dados atualizados do filme.</param>
    /// <returns><see langword="true"/> se atualizado; caso contrário, <see langword="false"/>.</returns>
    bool Update(Movie movie);

    /// <summary>Remove o filme com o identificador informado.</summary>
    /// <param name="id">Identificador do filme.</param>
    /// <returns><see langword="true"/> se removido; caso contrário, <see langword="false"/>.</returns>
    bool Delete(Guid id);
}
