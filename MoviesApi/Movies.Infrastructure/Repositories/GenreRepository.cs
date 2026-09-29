using Microsoft.EntityFrameworkCore;
using Movies.Application.Interfaces;
using Movies.Domain.Entities;
using Movies.Infrastructure.Data;

namespace Movies.Infrastructure.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly ApplicationDBContext _context;

    public GenreRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Genre>> GetAllAsync()
    {
        return await _context.Genres
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Genre?> GetByIdAsync(byte id)
    {
        return await _context.Genres
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task AddAsync(Genre genre)
    {
        await _context.Genres.AddAsync(genre);
    }

    public void Update(Genre genre)
    {
        _context.Genres.Update(genre);
    }

    public void Delete(Genre genre)
    {
        _context.Genres.Remove(genre);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}