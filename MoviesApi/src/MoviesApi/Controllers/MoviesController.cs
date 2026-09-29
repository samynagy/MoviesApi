using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Dtos;
using MoviesApi.Models;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private new List<string> _allowedExtenstions = new List<string>
        {
            ".jpg" , ".png"
        };
        private long _maxAllowedPostersize = 1 * 1024 * 1024;// 1 mb
        public MoviesController(ApplicationDBContext context)
        {
            _context = context;

        }
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _context.Movies.Include(m=>m.Genre).ToListAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieByIdAsync(int id)
        {
            var movie = await _context
                            .Movies
                            .Include(m=>m.Genre).
                            SingleOrDefaultAsync(m => m.Id == id);
            if(movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }
        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte Genreid)
        {
            var movies = await _context.Movies
                .Where(m => m.GenreId == Genreid)
                .Include(m => m.Genre)
                .ToListAsync();
            return Ok(movies);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm]MovieDto dto)
        {
            if(dto.Poster == null)
                return BadRequest("Poster is Required");


            /// need some validations 
            /// 
            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("invalid extenstion only png and jpg allowed");
            
            if(dto.Poster.Length > _maxAllowedPostersize)
                return BadRequest("max allowed size for poster is 1MB");

            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);

            if(!isValidGenre)
                return BadRequest("invalid Genre ID");


            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            var movie = new Movie
            {
                GenreId = dto.GenreId , 
                Poster = dataStream.ToArray() , 
                Rate = dto.Rate , 
                Storyline = dto.Storyline , 
                Tilte = dto.Tilte , 
                Year = dto.Year
            };

           await _context.Movies.AddAsync(movie);
            _context.SaveChanges();
            return Ok(movie);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id , [FromForm] MovieDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();
            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);

            if (!isValidGenre)
                return BadRequest("invalid Genre ID");



            if (dto.Poster != null) {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("invalid extenstion only png and jpg allowed");

                if (dto.Poster.Length > _maxAllowedPostersize)
                    return BadRequest("max allowed size for poster is 1MB");


                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }
            


            


            movie.GenreId = dto.GenreId;
            movie.Rate = dto.Rate;
            movie.Storyline = dto.Storyline;
            movie.Tilte = dto.Tilte;
            movie.Year = dto.Year;

            _context.SaveChanges();
            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();
             _context.Movies.Remove(movie);
            _context.SaveChanges();
            return Ok(movie);
        }
    }
}
