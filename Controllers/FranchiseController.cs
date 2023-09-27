using FilmAPI.Data.DTOs.Movies;
using FilmAPI.Data.DTOs.Characters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FilmAPI.Data.DTOs.Franchises;
using FilmAPI.Data.Models;
using FilmAPI.Services.Franchises;
using Microsoft.AspNetCore.Mvc;

namespace FilmAPI.Controllers
{
    [Route("api/v1/franchise")]
    [ApiController]
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
        public async Task<IActionResult> GetAll()
        {
            var franchises = await _franchiseService.GetAllAsync();
            var franchiseDTOs = _mapper.Map<IEnumerable<FranchiseDTO>>(franchises); // Map to DTO
            return Ok(franchiseDTOs); // Return DTOs
        }

        /// <summary>
        /// Get a franchise by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var franchise = await _franchiseService.GetByIdAsync(id);
            if (franchise == null)
            {
                return NotFound(); // 404 Not Found
            }

            var franchiseDTO = _mapper.Map<FranchiseDTO>(franchise); // Map to DTO
            return Ok(franchiseDTO); // Return DTO
        }

        /// <summary>
        /// Create a new franchise.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] FranchiseDTO franchiseDTO) // Use FranchiseDTO
        {
            if (franchiseDTO == null)
            {
                return BadRequest(); // 400 Bad Request
            }

            var franchise = _mapper.Map<Franchise>(franchiseDTO); // Map DTO to entity
            var savedFranchise = await _franchiseService.AddAsync(franchise);
            var savedFranchiseDTO = _mapper.Map<FranchiseDTO>(savedFranchise); // Map to DTO
            return CreatedAtAction(nameof(GetById), new { id = savedFranchiseDTO.Id }, savedFranchiseDTO);
        }

        /// <summary>
        /// Update an existing franchise by ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FranchiseDTO franchiseDTO) // Use FranchiseDTO
        {
            if (franchiseDTO == null || id != franchiseDTO.Id)
            {
                return BadRequest(); // 400 Bad Request
            }

            var existingFranchise = await _franchiseService.GetByIdAsync(id);
            if (existingFranchise == null)
            {
                return NotFound(); // 404 Not Found
            }

            var franchise = _mapper.Map<Franchise>(franchiseDTO); // Map DTO to entity
            await _franchiseService.UpdateAsync(franchise);
            return NoContent(); // 204 No Content
        }

        /// <summary>
        /// Delete a franchise by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingFranchise = await _franchiseService.GetByIdAsync(id);
            if (existingFranchise == null)
            {
                return NotFound(); // 404 Not Found
            }

            await _franchiseService.DeleteAsync(id);
            return NoContent(); // 204 No Content
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
