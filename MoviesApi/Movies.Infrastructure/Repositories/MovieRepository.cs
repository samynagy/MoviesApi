using Microsoft.EntityFrameworkCore;
using Movies.Application.Interfaces;
using Movies.Domain.Entities;
using Movies.Infrastructure.Data;

namespace Movies.Infrastructure.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDBContext _context;

    public MovieRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        return await _context.Movies
            .Include(m => m.Genre)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Movie?> GetByIdAsync(int id)
    {
        return await _context.Movies
            .Include(m => m.Genre)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Movie>> GetByGenreIdAsync(byte genreId)
    {
        return await _context.Movies
            .Where(m => m.GenreId == genreId)
            .Include(m => m.Genre)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<bool> GenreExistsAsync(byte genreId)
    {
        return _context.Genres.AnyAsync(g => g.Id == genreId);
    }

    public async Task AddAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
    }

    public void Delete(Movie movie)
    {
        _context.Movies.Remove(movie);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}