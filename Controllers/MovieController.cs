using AutoMapper;
using FilmAPI.Data.DTOs.Characters;
using FilmAPI.Data.DTOs.Movies;
using FilmAPI.Data.Exceptions;
using FilmAPI.Data.Models;
using FilmAPI.Services.Characters;
using FilmAPI.Services.Movies;
using Microsoft.AspNetCore.Mvc;

namespace FilmAPI.Controllers
{
    [Route("api/v1/movie")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;
        private readonly ICharacterService _characterService;

        public MovieController(IMovieService movieService, IMapper mapper, ICharacterService characterService)
        {
            _movieService = movieService;
            _mapper = mapper;
            _characterService = characterService;

        }

        /// <summary>
        /// Retrieve a list of all movies.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<MovieDto>>(await _movieService.GetAllAsync()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }



        /// <summary>
        /// Retrieve a movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetById(int id)
        {
            try
            {
                var movie = await _movieService.GetByIdAsync(id);

                if (movie == null)
                {
                    throw new MovieNotFound(id);
                }

                var movieDto = _mapper.Map<MovieDto>(movie);
                return Ok(movieDto);
            }
            catch (MovieNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred.");
            }
        }


        /// <summary>
        /// Add a new movie to the database.
        /// </summary>
        /// <param name="movieDto">The MovieDto object containing movie information.</param>
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] MovieDto movieDto)
        {
            try
            {
                // Map the MovieDto to a Movie entity for saving.
                var movie = _mapper.Map<Movie>(movieDto);
                // Add the movie to the database using the service.
                var addedMovie = await _movieService.AddAsync(movie);
                // Map the added movie entity to a MovieDto for the response.
                var addedMovieDto = _mapper.Map<MovieDto>(addedMovie);
                // Return a 201 Created response with the URL to retrieve the added movie.
                return CreatedAtAction(nameof(GetById), new { id = addedMovie.Id }, addedMovieDto);
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions with a 400 Bad Request response.
                return BadRequest($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to update.</param>
        /// <param name="movieDto">The MovieDto object containing updated movie information.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieDto movieDto)
        {
            try
            {
                // Retrieve the existing movie by its ID.
                var existingMovie = await _movieService.GetByIdAsync(id);
                if (existingMovie == null)
                {
                    // If the movie is not found, throw a custom exception.
                    throw new MovieNotFound(id);
                }

                // Update the existing movie entity with data from the MovieDto.
                _mapper.Map(movieDto, existingMovie);
                // Update the movie in the database using the service.
                var updatedMovie = await _movieService.UpdateAsync(existingMovie);
                // Map the updated movie entity to a MovieDto for the response.
                var updatedMovieDto = _mapper.Map<MovieDto>(updatedMovie);
                // Return the updated movie as JSON.
                return Ok(updatedMovieDto);
            }
            catch (MovieNotFound ex)
            {
                // Handle the custom exception by returning a 404 Not Found response.
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                //Handle unexpected exceptions with a 400 Bad Request response.
                return BadRequest($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Delete the movie by its ID using the service.
                await _movieService.DeleteAsync(id);
                // Return a 204 No Content response to indicate a successful deletion.
                return NoContent();
            }
            catch (MovieNotFound ex)
            {
                // Handle the custom exception by returning a 404 Not Found response.
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions with a 500 Internal Server Error response.
                return StatusCode(500, "An error occurred.");
            }
        }

        [HttpGet("{id}/characters")]
        public async Task<IActionResult> GetCharactersInMovie(int id)
        {
            try
            {
                // Retrieve character IDs associated with the movie.
                var characterIds = await _movieService.GetCharacterIdsInMovieAsync(id);

                // Retrieve character details using the CharacterService and character IDs.
                var characterDetails = new List<CharacterDTO>();
                foreach (var characterId in characterIds)
                {
                    var character = await _characterService.GetByIdAsync(characterId);
                    if (character != null)
                    {
                        // Map the character entity to a CharacterDto object.
                        var characterDto = _mapper.Map<CharacterDTO>(character);
                        characterDetails.Add(characterDto);
                    }
                }

                // Return the character details as JSON.
                return Ok(characterDetails);
            }
            catch (MovieNotFound ex)
            {
                // Handle the custom exception by returning a 404 Not Found response.
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions and return an appropriate response.
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }




    }
}

