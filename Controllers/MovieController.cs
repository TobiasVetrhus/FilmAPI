using FilmAPI.Data.Exceptions;
using FilmAPI.Data.Models;
using FilmAPI.DTOs;
using FilmAPI.Services.Movies;
using Microsoft.AspNetCore.Mvc;

namespace FilmAPI.Controllers
{
    [Route("api/v1/movie")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        private MovieDto MapMovieToDto(Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Genre = movie.Genre,
                ReleaseYear = movie.ReleaseYear,
                // Map other properties as needed
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //try
            //{
            var movies = await _movieService.GetAllAsync();
            var movieDtos = movies.Select(movie => MapMovieToDto(movie)).ToList();
            return Ok(movieDtos);
            //}
            //catch (Exception ex)
            //{
            //    return HandleException(ex);
            //}
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var movie = await _movieService.GetByIdAsync(id);
                if (movie == null)
                {
                    throw new MovieNotFoundException(id);
                }

                var movieDto = MapMovieToDto(movie);
                return Ok(movieDto);
            }
            catch (MovieNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] MovieDto movieDto)
        {
            try
            {
                // Map the MovieDto back to the Movie entity
                var movie = new Movie
                {
                    Title = movieDto.Title,
                    Genre = movieDto.Genre,
                    ReleaseYear = movieDto.ReleaseYear,
                    // Map other properties as needed
                };

                var addedMovie = await _movieService.AddAsync(movie);

                return CreatedAtAction(nameof(GetById), new { id = addedMovie.Id }, MapMovieToDto(addedMovie));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieDto movieDto)
        {
            try
            {
                var existingMovie = await _movieService.GetByIdAsync(id);
                if (existingMovie == null)
                {
                    throw new MovieNotFoundException(id);
                }

                // Map the properties from the DTO to the existing movie entity
                existingMovie.Title = movieDto.Title;
                existingMovie.Genre = movieDto.Genre;
                existingMovie.ReleaseYear = movieDto.ReleaseYear;
                // Map other properties as needed

                var updatedMovie = await _movieService.UpdateAsync(existingMovie);

                return Ok(MapMovieToDto(updatedMovie));
            }
            catch (MovieNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingMovie = await _movieService.GetByIdAsync(id);
                if (existingMovie == null)
                {
                    throw new MovieNotFoundException(id);
                }

                await _movieService.DeleteAsync(id);

                return NoContent();
            }
            catch (MovieNotFoundException ex)
            {
                return NotFound(ex.Message); // Movie not found
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Handle other exceptions and return a bad request response
            }
        }
    }
}
