using FilmAPI.Data.Models;

namespace FilmAPI.Services.Characters
{
    public interface ICharacterService : ICrudService<Character, int>
    {
        Task<IEnumerable<Character>> GetByNameAsync(string name);

        Task UpdateMoviesAsync(int characterId, int[] movieIds);
        Task<ICollection<Movie>> GetMoviesAsync(int id);

    }
}
