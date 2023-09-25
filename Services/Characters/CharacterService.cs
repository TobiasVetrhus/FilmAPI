using FilmAPI.Data.Exceptions;
using FilmAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

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
            obj.Movies.Clear();
            await _dbContext.Characters.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task DeleteAsync(int id)
        {
            if (!await CharacterExistsAsync(id))
                FailWithCharacterNotFound(id);

            var character = await _dbContext.Characters
                .Where(c => c.Id == id)
                .FirstAsync();

            character.Movies.Clear();
            _dbContext.Characters.Remove(character);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Character>> GetAllAsync()
        {
            return await _dbContext.Characters.ToListAsync();
        }

        public async Task<Character> GetByIdAsync(int id)
        {
            var character = await _dbContext.Characters
                .Where(c => c.Id == id)
                .FirstAsync();

            if (character is null)
                FailWithCharacterNotFound(id);

            return character;
        }

        public async Task<IEnumerable<Character>> GetByNameAsync(string name)
        {
            var characters = await _dbContext.Characters
                .Where(c => c.FullName.Contains(name))
                .ToListAsync();

            if (characters is null)
                FailWithCharacterNameNotFound(name);

            return characters;
        }

        public async Task<Character> UpdateAsync(Character obj)
        {
            if (!await CharacterExistsAsync(obj.Id))
                FailWithCharacterNotFound(obj.Id);

            obj.Movies.Clear();
            _dbContext.Entry(obj).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return obj;
        }

        private async Task<bool> CharacterExistsAsync(int id)
        {
            return await _dbContext.Characters.AnyAsync(c => c.Id == id);
        }

        private static void FailWithCharacterNotFound(int id)
        {
            throw new EntityNotFoundException(id);
        }

        private static void FailWithCharacterNameNotFound(string name)
        {
            throw new EntityNotFoundException(name);
        }

    }
}
