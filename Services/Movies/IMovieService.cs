using FilmAPI.Data.Models;

namespace FilmAPI.Services.Movies
{
    public interface IMovieService : ICrudService<Movie, int>
    {
        public interface IMovieService
        {
            Task<Movie> AddAsync(Movie obj);
            Task DeleteAsync(int id);
            Task<IEnumerable<Movie>> GetAllAsync();
            Task<Movie> GetByIdAsync(int id);
            Task UpdateAsync(Movie obj);
            Task UpdateCharactersAsync(ICollection<int> characterIds, int movieId);
            Task UpdateMoviesInFranchiseAsync(ICollection<int> movieIds, int franchiseId);
            Task<IEnumerable<Movie>> GetMoviesInFranchiseAsync(int franchiseId);
            Task<IEnumerable<Character>> GetCharactersInMovieAsync(int movieId);
            Task<IEnumerable<Character>> GetCharactersInFranchiseAsync(int franchiseId);
        }

    }
}
