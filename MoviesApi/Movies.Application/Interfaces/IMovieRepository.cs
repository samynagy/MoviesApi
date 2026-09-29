using Movies.Domain.Entities;

namespace Movies.Application.Interfaces;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAllAsync();

    Task<Movie?> GetByIdAsync(int id);

    Task<IEnumerable<Movie>> GetByGenreIdAsync(byte genreId);

    Task<bool> GenreExistsAsync(byte genreId);

    Task AddAsync(Movie movie);

    void Delete(Movie movie);

    Task<bool> SaveChangesAsync();
}