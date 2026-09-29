using Microsoft.AspNetCore.Mvc;
using Movies.Application.Services;
using Movies.Domain.Entities;
using MoviesApi.Dtos;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        private readonly List<string> _allowedExtensions = new()
        {
            ".jpg",
            ".png"
        };

        private const long MaxAllowedPosterSize = 1 * 1024 * 1024; // 1 MB

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movieService.GetAllAsync();
            var result = movies.Select(m => new 
            {
                Id = m.Id,
                Tilte = m.Tilte,
                Year = m.Year,
                Rate = m.Rate,
                Storyline = m.Storyline,
                GenreId = m.GenreId
            });
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);

            if (movie is null)
                return NotFound();

            return Ok(movie);
        }

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetMoviesByGenreId(byte genreId)
        {
            var movies = await _movieService.GetByGenreIdAsync(genreId);
            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if (dto.Poster is null)
                return BadRequest("Poster is required.");

            if (!IsValidPoster(dto.Poster))
                return BadRequest("Invalid poster. Only JPG and PNG files up to 1 MB are allowed.");

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            var movie = new Movie
            {
                GenreId = dto.GenreId,
                Poster = dataStream.ToArray(),
                Rate = dto.Rate,
                Storyline = dto.Storyline,
                Tilte = dto.Tilte,
                Year = dto.Year
            };

            try
            {
                var createdMovie = await _movieService.CreateAsync(movie);
                return Ok(createdMovie);
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid Genre ID.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
        {
            byte[]? poster = null;

            if (dto.Poster is not null)
            {
                if (!IsValidPoster(dto.Poster))
                    return BadRequest("Invalid poster. Only JPG and PNG files up to 1 MB are allowed.");

                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);

                poster = dataStream.ToArray();
            }

            var updatedMovie = new Movie
            {
                GenreId = dto.GenreId,
                Rate = dto.Rate,
                Storyline = dto.Storyline,
                Tilte = dto.Tilte,
                Year = dto.Year
            };

            try
            {
                var result = await _movieService.UpdateAsync(id, updatedMovie, poster);

                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid Genre ID.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deletedMovie = await _movieService.DeleteAsync(id);

            if (deletedMovie is null)
                return NotFound();

            return Ok(deletedMovie);
        }

        private bool IsValidPoster(IFormFile poster)
        {
            var extension = Path.GetExtension(poster.FileName).ToLowerInvariant();

            return _allowedExtensions.Contains(extension)
                   && poster.Length <= MaxAllowedPosterSize;
        }
    }
}