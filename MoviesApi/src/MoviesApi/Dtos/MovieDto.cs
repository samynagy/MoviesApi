using MoviesApi.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dtos
{
    public class MovieDto
    {
        [MaxLength(250)]
        public string Tilte { get; set; }

        public int Year { get; set; }
        public decimal Rate { get; set; }
        [MaxLength(500)]
        public string Storyline { get; set; }
        public IFormFile? Poster { get; set; }


        public byte GenreId { get; set; }

    }
}
