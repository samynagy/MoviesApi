using Movies.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;

namespace Movies.Infrastructure.Data;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(
        DbContextOptions<ApplicationDBContext> options
    ) : base(options)
    {
    }

    public DbSet<Genre> Genres { get; set; }

    public DbSet<Movie> Movies { get; set; }
}