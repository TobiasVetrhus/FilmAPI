using FilmAPI.Data.Models;
using FilmAPI.Services.Franchises;
using Microsoft.AspNetCore.Mvc;

namespace FilmAPI.Controllers
{
    [Route("api/v1/movie")]
    [ApiController]
    public class FranchiseController : ControllerBase
    {
        private readonly IFranchiseService _franchiseService;

        public FranchiseController(IFranchiseService franchiseService)
        {
            _franchiseService = franchiseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Franchise franchise)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
