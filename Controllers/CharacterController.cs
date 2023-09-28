using AutoMapper;
using FilmAPI.Data.DTOs.Characters;
using FilmAPI.Data.DTOs.Movies;
using FilmAPI.Data.Exceptions;
using FilmAPI.Data.Models;
using FilmAPI.Services.Characters;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace FilmAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;
        private readonly IMapper _mapper;

        public CharacterController(ICharacterService characterService, IMapper mapper)
        {
            _characterService = characterService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all characters.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacterDTO>>> GetCharacters()
        {
            var characters = await _characterService.GetAllAsync();
            var characterDTOs = _mapper.Map<IEnumerable<CharacterDTO>>(characters);
            return Ok(characterDTOs);
        }

        /// <summary>
        /// Gets a character by given id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterDTO>> GetCharacterById(int id)
        {
            try
            {
                var character = await _characterService.GetByIdAsync(id);
                return _mapper.Map<CharacterDTO>(character);
            }
            catch (CharacterNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Gets a character by given name.
        /// </summary>
        [HttpGet("byname/{name}")]
        public async Task<ActionResult<IEnumerable<CharacterDTO>>> GetCharacterByName(string name)
        {
            try
            {
                var characters = await _characterService.GetByNameAsync(name);
                var charactersDTOs = _mapper.Map<IEnumerable<CharacterDTO>>(characters);
                return Ok(charactersDTOs);
            }
            catch (CharacterNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Returns all movies associated with a character.
        /// </summary>
        [HttpGet("{characterId}/movies")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies(int characterId)
        {
            try
            {
                var movies = await _characterService.GetMoviesAsync(characterId);
                return Ok(_mapper.Map<IEnumerable<MovieDto>>(movies));
            }
            catch (CharacterNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new character.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CharacterDTO>> PostCharacter(CharacterPostDTO character)
        {
            var newCharacter = await _characterService.AddAsync(_mapper.Map<Character>(character));

            await _characterService.UpdateMoviesAsync(newCharacter.Id, character.MovieIds);

            return CreatedAtAction("GetCharacterById", new { id = newCharacter.Id }, _mapper.Map<CharacterDTO>(newCharacter));
        }

        /// <summary>
        /// Updates a character.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacter(int id, CharacterPutDTO character)
        {
            if (id != character.Id)
            {
                return BadRequest();
            }

            try
            {
                var updatedCharacter = await _characterService.UpdateAsync(_mapper.Map<Character>(character));
                await _characterService.UpdateMoviesAsync(updatedCharacter.Id, character.MovieIds);
            }
            catch (CharacterNotFound ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Updates a character's movies.
        /// </summary>
        [HttpPut("{id}/movies")]
        public async Task<IActionResult> UpdateMovies(int id, [FromBody] int[] movies)
        {
            try
            {
                await _characterService.UpdateMoviesAsync(id, movies);
                return NoContent();
            }
            catch (CharacterNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a character.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            try
            {
                await _characterService.DeleteAsync(id);
                return NoContent();
            }
            catch (CharacterNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}