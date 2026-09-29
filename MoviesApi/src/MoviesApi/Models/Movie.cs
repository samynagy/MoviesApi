using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string Tilte { get; set; }

        public int Year { get; set; }
        public decimal Rate { get; set; }
        [MaxLength(500)]
        public string Storyline { get; set; }
        public byte[] Poster { get; set; }


        public byte GenreId { get; set; }
        public Genre Genre { get; set; }


    }
}
