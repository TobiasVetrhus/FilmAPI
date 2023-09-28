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

        
        // Add a new franchise to the database.
        public async Task<Franchise> AddAsync(Franchise obj)
        {
            await _dbContext.Franchises.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

   
        // Get all franchises from the database.
  
        public async Task<IEnumerable<Franchise>> GetAllAsync()
        {
            // Retrieve all franchises including associated movies.
            return await _dbContext.Franchises
                .Include(f => f.Movies)
                .ToListAsync();
        }


        // Get a franchise by ID from the database.
       
        public async Task<Franchise> GetByIdAsync(int id)
        {
            // Check if the franchise with the given ID exists.
            if (!await FranchiseExistsAsync(id))
                throw new FranchiseNotFound(id);
            
            // Retrieve the franchise by ID including associated movies.
            var franchise = await _dbContext.Franchises
                .Where(f => f.Id == id)
                .Include(f => f.Movies)
                .FirstAsync();

            return franchise;
        }


        
        // Update an existing franchise in the database.
        public async Task<Franchise> UpdateAsync(Franchise obj)
        {
            if (!await FranchiseExistsAsync(obj.Id))
                throw new FranchiseNotFound(obj.Id);

            _dbContext.Entry(obj).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return obj;
        }


       
        // Delete a franchise by ID.
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



        
        //Get characters belonging to a franchise by franchise ID. 
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

      
        // Get movies belonging to a franchise by franchise ID.
        public async Task<ICollection<Movie>> GetFranchiseMoviesAsync(int franchiseId)
        {
            if (!await FranchiseExistsAsync(franchiseId))
                throw new FranchiseNotFound(franchiseId);

            var franchise = await _dbContext.Franchises
                .Include(f => f.Movies)
                .FirstOrDefaultAsync(f => f.Id == franchiseId);

            return franchise.Movies.ToList();
        }

        // Update the list of movies associated with a franchise.
       
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

       
        // Get a franchise by ID.
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

