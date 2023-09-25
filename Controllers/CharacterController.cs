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
        public async Task<ActionResult<IEnumerable<Character>>> GetCharacters()
        {
            return Ok(await _characterService.GetAllAsync());
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
        public async Task<ActionResult<IEnumerable<Character>>> GetCharacterByName(string name)
        {
            try
            {
                var characters = await _characterService.GetByNameAsync(name);
                return Ok(characters);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Character>> PostCharacter(Character character)
        {
            await _characterService.AddAsync(character);
            return CreatedAtAction("GetCharacterById", new { id = character.Id }, character);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacter(int id, Character character)
        {
            if (id != character.Id)
            {
                return BadRequest();
            }

            try
            {
                await _characterService.UpdateAsync(character);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
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