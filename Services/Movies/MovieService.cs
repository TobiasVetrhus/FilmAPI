using FilmAPI.Data.Models;

namespace FilmAPI.Services.Movies
{
    public class MovieService : IMovieService
    {
        private readonly MoviesDbContext _dbContext;

        public MovieService(MoviesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Movie> AddAsync(Movie obj)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Character>> GetMovieCharactersAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public async Task<Movie> UpdateAsync(Movie obj)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCharactersAsync(ICollection<int> characterIds, int movieId)
        {
            throw new NotImplementedException();
        }
    }
}
