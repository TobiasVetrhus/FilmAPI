using AutoMapper;
using FilmAPI.Data.DTOs.Characters;
using FilmAPI.Data.DTOs.Franchises;
using FilmAPI.Data.DTOs.Movies;
using FilmAPI.Data.Exceptions;
using FilmAPI.Data.Models;
using FilmAPI.Services.Franchises;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace FilmAPI.Controllers
{
    [Route("api/v1/franchise")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class FranchiseController : ControllerBase
    {
        private readonly IFranchiseService _franchiseService;
        private readonly IMapper _mapper; // Inject IMapper

        public FranchiseController(IFranchiseService franchiseService, IMapper mapper)
        {
            _franchiseService = franchiseService;
            _mapper = mapper; // Inject IMapper
        }

        /// <summary>
        /// Get all franchises.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FranchiseDTO>>> GetAll()
        {
            var franchises = await _franchiseService.GetAllAsync();
            var franchiseDTOs = _mapper.Map<IEnumerable<FranchiseDTO>>(franchises); // Map to DTO
            return Ok(franchiseDTOs); // Return DTOs
        }

        /// <summary>
        /// Get a franchise by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<FranchiseDTO>> GetById(int id)
        {
            try
            {
                var franchise = await _franchiseService.GetByIdAsync(id);
                return _mapper.Map<FranchiseDTO>(franchise);
            }
            catch (FranchiseNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Create a new franchise.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FranchiseDTO>> PostFranchise(FranchisePostDTO franchise) // Use FranchiseDTO
        {
            var newFranchise = await _franchiseService.AddAsync(_mapper.Map<Franchise>(franchise));

            return CreatedAtAction("GetById", new { id = newFranchise.Id }, _mapper.Map<FranchiseDTO>(newFranchise));
        }

        /// <summary>
        /// Update an existing franchise by ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, FranchisePutDTO franchise) // Use FranchiseDTO
        {
            if (id != franchise.Id)
            {
                return BadRequest();
            }

            try
            {
                var updatedFranchise = await _franchiseService.UpdateAsync(_mapper.Map<Franchise>(franchise));

            }
            catch (FranchiseNotFound ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete a franchise by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _franchiseService.DeleteAsync(id);
                return NoContent();
            }
            catch (FranchiseNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get all movies in a franchise by franchise ID.
        /// </summary>
        [HttpGet("{id}/movies")]
        public async Task<IActionResult> GetFranchiseMovies(int id)
        {
            var movies = await _franchiseService.GetFranchiseMoviesAsync(id);
            if (movies == null)
            {
                return NotFound();
            }

            var movieDTOs = _mapper.Map<IEnumerable<MovieDto>>(movies);
            return Ok(movieDTOs);
        }
        /// <summary>
        /// Update the list of movies associated with a franchise by franchise ID.
        /// </summary>
        [HttpPut("{id}/movies")]
        public async Task<IActionResult> PutFranchiseMovies(int id, [FromBody] int[] movieIds)
        {
            try
            {
                await _franchiseService.UpdateMoviesAsync(id, movieIds);
                return NoContent();
            }
            catch (FranchiseNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (MovieNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions and return an appropriate response.
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        /// <summary>
        /// Get all characters in a franchise by franchise ID.
        /// </summary>
        [HttpGet("{id}/characters")]
        public async Task<IActionResult> GetFranchiseCharacters(int id)
        {
            var characters = await _franchiseService.GetFranchiseCharactersAsync(id);
            if (characters == null)
            {
                return NotFound();
            }

            var characterDTOs = _mapper.Map<IEnumerable<CharacterDTO>>(characters);
            return Ok(characterDTOs);
        }
    }
}
