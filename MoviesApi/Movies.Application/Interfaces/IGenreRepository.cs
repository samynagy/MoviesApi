using Movies.Domain.Entities;

namespace Movies.Application.Interfaces;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAllAsync();

    Task<Genre?> GetByIdAsync(byte id);

    Task AddAsync(Genre genre);

    void Update(Genre genre);

    void Delete(Genre genre);

    Task<bool> SaveChangesAsync();
}