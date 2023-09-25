using FilmAPI.Data.Models;

namespace FilmAPI.Services.Characters
{
    public interface ICharacterService : ICrudService<Character, int>
    {
        Task<Character> GetByNameAsync(string name);

    }
}
