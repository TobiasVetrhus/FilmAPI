using FilmAPI.Data.Models;

namespace FilmAPI.Services.Franchises
{
    public interface IFranchiseService : ICrudService<Franchise, int>
    {
        Task UpdateMoviesAsync(ICollection<int> movieIds, int franchiseId);
        Task<ICollection<Movie>> GetFranchiseMoviesAsync(int franchiseId);
        Task<ICollection<Character>> GetFranchiseCharactersAsync(int franchiseId);
    }
}
