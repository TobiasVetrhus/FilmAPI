using FilmAPI.Data.Exceptions;
using FilmAPI.Data.Models;

namespace FilmAPI.Services.Characters
{
    public interface ICharacterService : ICrudService<Character, int>
    {
        /// <summary>
        /// Returns a list of characters by the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <returns></returns>
        Task<IEnumerable<Character>> GetByNameAsync(string name);

        /// <summary>
        /// Updates the movies for a character.
        /// This replaces all the related movies.
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="movieIds"></param>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <returns></returns>
        Task UpdateMoviesAsync(int characterId, int[] movieIds);
    }
}
