using AutoMapper;
using FilmAPI.Data.DTOs.Characters;
using FilmAPI.Data.DTOs.Movies;
using FilmAPI.Data.Exceptions;
using FilmAPI.Data.Models;
using FilmAPI.Services.Characters;
using FilmAPI.Services.Movies;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace FilmAPI.Controllers
{

    /// <summary>
    /// Controller for managing movie-related operations.
    /// </summary>
    [Route("api/v1/movie")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(DefaultApiConventions))]
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
            return Ok(_mapper.Map<IEnumerable<MovieDto>>(await _movieService.GetAllAsync()));
        }


        /// <summary>
        /// Add a new movie.
        /// </summary>
        /// <param name="movie">The movie data to be added.</param>
        /// <returns>NoContent if successful, NotFound if the franchise is not found.</returns>
        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie(MovieDtoPost movie)
        {
            try
            {
                var newMovie = await _movieService.AddAsync(_mapper.Map<Movie>(movie));

                CreatedAtAction("GetById",
                    new { id = newMovie.Id },
                   _mapper.Map<MovieDto>(newMovie));
                return NoContent();
            }
            catch (FranchiseNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }


        /// <summary>
        /// Get a movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve.</param>
        /// <returns>The movie if found, NotFound if not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetById(int id)
        {
            try
            {
                var movie = await _movieService.GetByIdAsync(id);
                var movieDto = _mapper.Map<MovieDto>(movie);
                return Ok(movieDto);
            }
            catch (MovieNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }


        /// <summary>
        /// Update an existing movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to update.</param>
        /// <param name="movieDto">The MovieDto object containing updated movie information.</param>
        /// <returns>NoContent if successful, NotFound if the movie is not found.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieDtoPut movieDto)
        {
            try
            {
                await _movieService.UpdateAsync(_mapper.Map(movieDto, await _movieService.GetByIdAsync(id)));
                return NoContent();
            }
            catch (MovieNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }


        /// <summary>
        /// Delete a movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        /// <returns>NoContent if successful, NotFound if the movie is not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _movieService.DeleteAsync(id);
                return NoContent();
            }
            catch (MovieNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }


        /// <summary>
        /// Get the characters in a movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve characters for.</param>
        /// <returns>A list of character details if successful, NotFound if the movie is not found.</returns>
        [HttpGet("{id}/characters")]
        public async Task<IActionResult> GetCharactersInMovie(int id)
        {
            try
            {
                var characterIds = await _movieService.GetCharacterIdsInMovieAsync(id);
                var characterDetails = new List<CharacterDTO>();
                foreach (var characterId in characterIds)
                {
                    var character = await _characterService.GetByIdAsync(characterId);
                    if (character != null)
                    {
                        var characterDto = _mapper.Map<CharacterDTO>(character);
                        characterDetails.Add(characterDto);
                    }
                }
                return Ok(characterDetails);
            }
            catch (MovieNotFound ex)
            {
                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Update the characters associated with a movie.
        /// </summary>
        /// <param name="movieId">The ID of the movie to update characters for.</param>
        /// <param name="characterIds">A list of character IDs to associate with the movie.</param>
        /// <returns>NoContent if successful, NotFound if the movie or characters are not found.</returns>
        [HttpPut("{movieId}/characters")]
        public async Task<IActionResult> PutCharactersInMovie(int movieId, [FromBody] List<int> characterIds)
        {
            try
            {
                await _movieService.UpdateMovieCharacterAsync(movieId, characterIds);
                return NoContent();
            }
            catch (MovieNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (CharacterNotFound ex)
            {
                return NotFound(ex.Message);
            }

        }


    }
}