using FilmAPI.Data.Models;

namespace FilmAPI.Services.Franchises
{
    public class FranchiseService : IFranchiseService
    {
        private readonly MoviesDbContext _dbContext;

        public FranchiseService(MoviesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Franchise> AddAsync(Franchise obj)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Franchise>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Franchise> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Character>> GetFranchiseCharactersAsync(int franchiseId)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Movie>> GetFranchiseMoviesAsync(int franchiseId)
        {
            throw new NotImplementedException();
        }

        public async Task<Franchise> UpdateAsync(Franchise obj)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateMoviesAsync(ICollection<int> movieIds, int franchiseId)
        {
            throw new NotImplementedException();
        }
    }
}
