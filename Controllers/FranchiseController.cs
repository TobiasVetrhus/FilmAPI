using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public FranchiseController(IFranchiseService franchiseService)
        {
            _franchiseService = franchiseService;
        }

        /// <summary>
        /// Get all franchises.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var franchises = await _franchiseService.GetAllAsync();
            return Ok(franchises);
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

            return Ok(franchise);
        }

        /// <summary>
        /// Create a new franchise.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Franchise franchise)
        {
            if (franchise == null)
            {
                return BadRequest(); // 400 Bad Request
            }

            var savedFranchise = await _franchiseService.AddAsync(franchise);
            return CreatedAtAction(nameof(GetById), new { id = savedFranchise.Id }, savedFranchise);
        }

        /// <summary>
        /// Update an existing franchise by ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Franchise franchise)
        {
            if (franchise == null || id != franchise.Id)
            {
                return BadRequest(); // 400 Bad Request
            }

            var existingFranchise = await _franchiseService.GetByIdAsync(id);
            if (existingFranchise == null)
            {
                return NotFound(); // 404 Not Found
            }

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
    }
}
