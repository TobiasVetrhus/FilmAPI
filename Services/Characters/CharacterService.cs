﻿using FilmAPI.Data.Exceptions;
using FilmAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace FilmAPI.Services.Characters
{
    public class CharacterService : ICharacterService
    {
        //Dependency Injection for DbContext
        private readonly MoviesDbContext _dbContext;

        public CharacterService(MoviesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Character> AddAsync(Character obj)
        {
            await _dbContext.Characters.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task DeleteAsync(int id)
        {
            //Check if the character with the given 'id' exists
            if (!await CharacterExistsAsync(id))
                throw new CharacterNotFound(id);

            //Retrieve the character and remove it from the DbContext
            var character = await _dbContext.Characters
                .Where(c => c.Id == id)
                .FirstAsync();

            _dbContext.Characters.Remove(character);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Character>> GetAllAsync()
        {
            return await _dbContext.Characters.Include(c => c.Movies).ToListAsync();
        }

        public async Task<Character> GetByIdAsync(int id)
        {
            if (!await CharacterExistsAsync(id))
                throw new CharacterNotFound(id);

            var character = await _dbContext.Characters
                .Where(c => c.Id == id)
                .Include(c => c.Movies)
                .FirstAsync();

            return character;
        }

        public async Task<IEnumerable<Character>> GetByNameAsync(string name)
        {
            var characters = await _dbContext.Characters
                .Where(c => c.FullName.Contains(name))
                .Include(c => c.Movies)
                .ToListAsync();

            if (!characters.Any())
                throw new CharacterNotFound(name);

            return characters;
        }

        public async Task<ICollection<Movie>> GetMoviesAsync(int characterId)
        {
            if (!await CharacterExistsAsync(characterId))
                throw new CharacterNotFound(characterId);

            var character = await _dbContext.Characters
                .Include(c => c.Movies)
                .FirstOrDefaultAsync(c => c.Id == characterId);

            return character.Movies.ToList();
        }

        public async Task<Character> UpdateAsync(Character obj)
        {
            if (!await CharacterExistsAsync(obj.Id))
                throw new CharacterNotFound(obj.Id);

            _dbContext.Entry(obj).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return obj;
        }

        public async Task UpdateMoviesAsync(int characterId, int[] movieIds)
        {
            //Retrieves the character by 'characterId' and includes their associated movies
            var character = await _dbContext.Characters
                .Include(c => c.Movies)
                .FirstOrDefaultAsync(c => c.Id == characterId);

            if (character != null)
            {
                character.Movies.Clear(); //Clears the characters movie collection

                //Iterate through the 'movieIds' and add corresponding movies to the character.
                foreach (int id in movieIds)
                {
                    if (!await MovieExistsAsync(id))
                        throw new MovieNotFound(id);

                    var movie = await _dbContext.Movies.FindAsync(id);
                    character.Movies.Add(movie);
                }

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new CharacterNotFound(characterId);
            }
        }

        //Helper methods to check if a character or movie exists by Id.
        private async Task<bool> CharacterExistsAsync(int id)
        {
            return await _dbContext.Characters.AnyAsync(c => c.Id == id);
        }
        private async Task<bool> MovieExistsAsync(int id)
        {
            return await _dbContext.Movies.AnyAsync(m => m.Id == id);
        }
    }
}
