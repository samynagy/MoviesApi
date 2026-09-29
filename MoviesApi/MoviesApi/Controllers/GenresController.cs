using Microsoft.AspNetCore.Mvc;
using Movies.Application.Services;
using MoviesApi.Dtos;

namespace MoviesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var genres = await _genreService.GetAllAsync();

        return Ok(genres);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] GenreDto dto)
    {
        var genre = new Movies.Domain.Entities.Genre
        {
            Name = dto.Name
        };

        var createdGenre = await _genreService.CreateAsync(genre);

        return Ok(createdGenre);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(byte id, [FromBody] GenreDto dto)
    {
        var updated = await _genreService.UpdateAsync(id, new Movies.Domain.Entities.Genre
        {
            Name = dto.Name
        });

        if (!updated)
        {
            return NotFound($"No genre was found with id: {id}");
        }

        return Ok(new
        {
            Id = id,
            Name = dto.Name
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(byte id)
    {
        var deleted = await _genreService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound($"No genre was found with id: {id}");
        }

        return NoContent();
    }
}