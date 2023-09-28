using FilmAPI.Data.Exceptions;
using FilmAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmAPI.Services.Franchises
{
    public class FranchiseService : IFranchiseService
    {
        private readonly MoviesDbContext _dbContext;

        public FranchiseService(MoviesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Add a new franchise to the database.
        /// </summary>
        public async Task<Franchise> AddAsync(Franchise obj)
        {
            await _dbContext.Franchises.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        /// <summary>
        /// Get all franchises from the database.
        /// </summary>
        public async Task<IEnumerable<Franchise>> GetAllAsync()
        {
            return await _dbContext.Franchises
                .Include(f => f.Movies)
                .ToListAsync();
        }

        /// <summary>
        /// Get a franchise by ID from the database.
        /// </summary>
        public async Task<Franchise> GetByIdAsync(int id)
        {
            if (!await FranchiseExistsAsync(id))
                throw new FranchiseNotFound(id);

            var franchise = await _dbContext.Franchises
                .Where(f => f.Id == id)
                .Include(f => f.Movies)
                .FirstAsync();

            return franchise;
        }


        /// <summary>
        /// Update an existing franchise in the database.
        /// </summary>
        public async Task<Franchise> UpdateAsync(Franchise obj)
        {
            if (!await FranchiseExistsAsync(obj.Id))
                throw new FranchiseNotFound(obj.Id);

            _dbContext.Entry(obj).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return obj;
        }


        /// <summary>
        /// Delete a franchise by ID.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            if (!await FranchiseExistsAsync(id))
                throw new FranchiseNotFound(id);

            var franchise = await _dbContext.Franchises
                .Include(f => f.Movies)
                .FirstOrDefaultAsync(f => f.Id == id);

            franchise.Movies.Clear();
            _dbContext.Franchises.Remove(franchise);
            await _dbContext.SaveChangesAsync();
        }



        /// <summary>
        /// Get characters belonging to a franchise by franchise ID.
        /// </summary>
        public async Task<ICollection<Character>> GetFranchiseCharactersAsync(int franchiseId)
        {
            if (!await FranchiseExistsAsync(franchiseId))
                throw new FranchiseNotFound(franchiseId);

            var franchise = await _dbContext.Franchises
                .Include(f => f.Movies)
                .ThenInclude(m => m.Characters)
                .FirstOrDefaultAsync(f => f.Id == franchiseId);

            return franchise.Movies.SelectMany(m => m.Characters).ToList();
        }

        /// <summary>
        /// Get movies belonging to a franchise by franchise ID.
        /// </summary>
        public async Task<ICollection<Movie>> GetFranchiseMoviesAsync(int franchiseId)
        {
            if (!await FranchiseExistsAsync(franchiseId))
                throw new FranchiseNotFound(franchiseId);

            var franchise = await _dbContext.Franchises
                .Include(f => f.Movies)
                .FirstOrDefaultAsync(f => f.Id == franchiseId);

            return franchise.Movies.ToList();
        }

        /// <summary>
        /// Update the list of movies associated with a franchise.
        /// </summary>
        public async Task UpdateMoviesAsync(int franchiseId, int[] movieIds)
        {
            //Retrieves the franchise by 'franchiseId' and includes their associated movies
            var franchise = await _dbContext.Franchises
                .Include(f => f.Movies)
                .FirstOrDefaultAsync(f => f.Id == franchiseId);

            if (franchise != null)
            {
                franchise.Movies.Clear(); //Clears the characters movie collection

                //Iterate through the 'movieIds' and add corresponding movies to the franchise.
                foreach (int id in movieIds)
                {
                    if (!await MovieExistsAsync(id))
                        throw new MovieNotFound(id);

                    var movie = await _dbContext.Movies.FindAsync(id);
                    franchise.Movies.Add(movie);
                }

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new FranchiseNotFound(franchiseId);
            }

        }

        /// <summary>
        /// Get a franchise by ID.
        /// </summary>
        public async Task<Franchise> GetFranchiseByIdAsync(int id)
        {
            return await _dbContext.Franchises.FindAsync(id);
        }

        private async Task<bool> FranchiseExistsAsync(int id)
        {
            return await _dbContext.Franchises.AnyAsync(f => f.Id == id);
        }
        private async Task<bool> MovieExistsAsync(int id)
        {
            return await _dbContext.Movies.AnyAsync(m => m.Id == id);
        }
    }


}

