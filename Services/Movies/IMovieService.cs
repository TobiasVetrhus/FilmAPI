using FilmAPI.Data.Models;

namespace FilmAPI.Services.Movies
{
    public interface IMovieService : ICrudService<Movie, int>
    {
        Task UpdateCharactersAsync(ICollection<int> characterIds, int movieId);
        Task<ICollection<Character>> GetMovieCharactersAsync(int movieId);
    }
}
