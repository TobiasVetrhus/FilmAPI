using FilmAPI.Data.Models;

namespace FilmAPI.Services.Movies
{
    public interface IMovieService : ICrudService<Movie, int>
    {

        //Task DeleteAsync(int id);
        //Task<IEnumerable<Movie>> GetAllAsync();

        //Task<Movie> GetByIdAsync(int id);
        //Task<Movie> UpdateAsync(Movie obj);
        Task<IEnumerable<int>> GetCharacterIdsInMovieAsync(int movieId);
    }
}
