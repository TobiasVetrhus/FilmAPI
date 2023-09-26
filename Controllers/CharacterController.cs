using AutoMapper;
using FilmAPI.Data.DTOs.Characters;
using FilmAPI.Data.Exceptions;
using FilmAPI.Data.Models;
using FilmAPI.Services.Characters;
using Microsoft.AspNetCore.Mvc;

namespace FilmAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;
        private readonly IMapper _mapper;

        public CharacterController(ICharacterService characterService, IMapper mapper)
        {
            _characterService = characterService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacterDTO>>> GetCharacters()
        {
            var characters = await _characterService.GetAllAsync();
            var characterDTOs = _mapper.Map<IEnumerable<CharacterDTO>>(characters);
            return Ok(characterDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterDTO>> GetCharacterById(int id)
        {
            try
            {
                var character = await _characterService.GetByIdAsync(id);
                return _mapper.Map<CharacterDTO>(character);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("byname/{name}")]
        public async Task<ActionResult<IEnumerable<CharacterDTO>>> GetCharacterByName(string name)
        {
            try
            {
                var characters = await _characterService.GetByNameAsync(name);
                var charactersDTOs = _mapper.Map<IEnumerable<CharacterDTO>>(characters);
                return Ok(charactersDTOs);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CharacterDTO>> PostCharacter(CharacterPostDTO character)
        {
            var newCharacter = await _characterService.AddAsync(_mapper.Map<Character>(character));

            return CreatedAtAction("GetCharacterById", new { id = newCharacter.Id }, _mapper.Map<CharacterDTO>(newCharacter));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacter(int id, CharacterPutDTO character)
        {
            if (id != character.Id)
            {
                return BadRequest();
            }

            try
            {
                await _characterService.UpdateAsync(_mapper.Map<Character>(character));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }

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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            try
            {
                await _characterService.DeleteAsync(id);
                return NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}