using FilmAPI.Data.Exceptions;
using FilmAPI.Data.Models;
using FilmAPI.DTOs;
using FilmAPI.Services.Movies;
using Microsoft.AspNetCore.Mvc;

namespace FilmAPI.Controllers
{
    /// <summary>
    /// Controller for managing movie-related operations.
    /// </summary>
    [Route("api/v1/movie")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieController"/> class.
        /// </summary>
        /// <param name="movieService">The movie service.</param>
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        /// <summary>
        /// Maps a <see cref="Movie"/> object to a <see cref="MovieDto"/> object.
        /// </summary>
        /// <param name="movie">The movie to map.</param>
        /// <returns>The mapped <see cref="MovieDto"/>.</returns>
        private MovieDto MapMovieToDto(Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Genre = movie.Genre,
                ReleaseYear = movie.ReleaseYear,
                Director = movie.Director,
                Picture = movie.Picture,
                Trailer = movie.Trailer,
            };
        }


        /// <summary>
        /// Gets a list of all movies.
        /// </summary>
        /// <returns>A list of movie DTOs.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var movies = await _movieService.GetAllAsync();
                var movieDtos = movies.Select(movie => MapMovieToDto(movie)).ToList();
                return Ok(movieDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets a movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve.</param>
        /// <returns>The movie DTO.</returns>
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

        /// <summary>
        /// Saves a new movie.
        /// </summary>
        /// <param name="movieDto">The movie DTO to save.</param>
        /// <returns>The created movie DTO.</returns>
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
                    Director = movieDto.Director,
                    Picture = movieDto.Picture,
                    Trailer = movieDto.Trailer,
                };

                var addedMovie = await _movieService.AddAsync(movie);

                return CreatedAtAction(nameof(GetById), new { id = addedMovie.Id }, MapMovieToDto(addedMovie));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing movie.
        /// </summary>
        /// <param name="id">The ID of the movie to update.</param>
        /// <param name="movieDto">The updated movie DTO.</param>
        /// <returns>The updated movie DTO.</returns>
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
                existingMovie.Director = movieDto.Director;
                existingMovie.Picture = movieDto.Picture;
                existingMovie.Trailer = movieDto.Trailer;

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

        /// <summary>
        /// Deletes a movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        /// <returns>No content.</returns>
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
