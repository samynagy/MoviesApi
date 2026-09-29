using Movies.Domain.Entities;

namespace Movies.Application.Services;

public interface IMovieService
{
    Task<IEnumerable<Movie>> GetAllAsync();

    Task<Movie?> GetByIdAsync(int id);

    Task<IEnumerable<Movie>> GetByGenreIdAsync(byte genreId);

    Task<Movie> CreateAsync(Movie movie);

    Task<bool> UpdateAsync(int id, Movie updatedMovie, byte[]? poster);

    Task<Movie?> DeleteAsync(int id);
}