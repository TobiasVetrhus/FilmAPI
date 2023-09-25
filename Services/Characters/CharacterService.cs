using FilmAPI.Data.Models;

namespace FilmAPI.Services.Characters
{
    public class CharacterService : ICharacterService
    {
        private readonly MoviesDbContext _dbContext;

        public CharacterService(MoviesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Character> AddAsync(Character obj)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Character>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Character> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Character> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Character> UpdateAsync(Character obj)
        {
            throw new NotImplementedException();
        }
    }
}
