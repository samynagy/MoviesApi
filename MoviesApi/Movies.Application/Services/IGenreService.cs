using Movies.Domain.Entities;

namespace Movies.Application.Services;

public interface IGenreService
{
    Task<IEnumerable<Genre>> GetAllAsync();

    Task<Genre?> GetByIdAsync(byte id);

    Task<Genre> CreateAsync(Genre genre);

    Task<bool> UpdateAsync(byte id, Genre updatedGenre);

    Task<bool> DeleteAsync(byte id);
}