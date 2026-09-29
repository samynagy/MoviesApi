using Movies.Application.Interfaces;
using Movies.Domain.Entities;

namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        return _movieRepository.GetAllAsync();
    }

    public Task<Movie?> GetByIdAsync(int id)
    {
        return _movieRepository.GetByIdAsync(id);
    }

    public Task<IEnumerable<Movie>> GetByGenreIdAsync(byte genreId)
    {
        return _movieRepository.GetByGenreIdAsync(genreId);
    }

    public async Task<Movie> CreateAsync(Movie movie)
    {
        if (!await _movieRepository.GenreExistsAsync(movie.GenreId))
        {
            throw new ArgumentException("Invalid GenreId.");
        }

        await _movieRepository.AddAsync(movie);
        await _movieRepository.SaveChangesAsync();

        return movie;
    }

    public async Task<bool> UpdateAsync(
        int id,
        Movie updatedMovie,
        byte[]? poster)
    {
        var movie = await _movieRepository.GetByIdAsync(id);

        if (movie is null)
            return false;

        if (!await _movieRepository.GenreExistsAsync(updatedMovie.GenreId))
        {
            throw new ArgumentException("Invalid GenreId.");
        }

        movie.Tilte = updatedMovie.Tilte;
        movie.Storyline = updatedMovie.Storyline;
        movie.Year = updatedMovie.Year;
        movie.Rate = updatedMovie.Rate;
        movie.GenreId = updatedMovie.GenreId;

        if (poster is not null)
        {
            movie.Poster = poster;
        }

        await _movieRepository.SaveChangesAsync();

        return true;
    }

    public async Task<Movie?> DeleteAsync(int id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);

        if (movie is null)
            return null;

        _movieRepository.Delete(movie);
        await _movieRepository.SaveChangesAsync();

        return movie;
    }
}