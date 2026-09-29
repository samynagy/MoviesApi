
using System.ComponentModel.DataAnnotations;

namespace Movies.Domain.Entities;

public class Movie
{
    public int Id { get; set; }

    [MaxLength(250)]
    public string Tilte { get; set; } = string.Empty;

    public int Year { get; set; }

    public decimal Rate { get; set; }

    [MaxLength(500)]
    public string Storyline { get; set; } = string.Empty;

    public byte[] Poster { get; set; } = Array.Empty<byte>();

    public byte GenreId { get; set; }

    public Genre Genre { get; set; } = null!;
}