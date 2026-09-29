using Movies.Application.Interfaces;
using Movies.Domain.Entities;

namespace Movies.Application.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public Task<IEnumerable<Genre>> GetAllAsync()
    {
        return _genreRepository.GetAllAsync();
    }

    public Task<Genre?> GetByIdAsync(byte id)
    {
        return _genreRepository.GetByIdAsync(id);
    }

    public async Task<Genre> CreateAsync(Genre genre)
    {
        await _genreRepository.AddAsync(genre);
        await _genreRepository.SaveChangesAsync();

        return genre;
    }

    public async Task<bool> UpdateAsync(byte id, Genre updatedGenre)
    {
        var existingGenre = await _genreRepository.GetByIdAsync(id);

        if (existingGenre is null)
            return false;

        existingGenre.Name = updatedGenre.Name;

        _genreRepository.Update(existingGenre);

        return await _genreRepository.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(byte id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);

        if (genre is null)
            return false;

        _genreRepository.Delete(genre);

        return await _genreRepository.SaveChangesAsync();
    }
}