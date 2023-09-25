using FilmAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        /// Add a new franchise.
        /// </summary>
        public async Task<Franchise> AddAsync(Franchise obj)
        {
            _dbContext.Franchises.Add(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        /// <summary>
        /// Delete a franchise by ID.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var franchise = await _dbContext.Franchises.FindAsync(id);
            if (franchise != null)
            {
                _dbContext.Franchises.Remove(franchise);
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Get all franchises.
        /// </summary>
        public async Task<IEnumerable<Franchise>> GetAllAsync()
        {
            return await _dbContext.Franchises.ToListAsync();
        }

        /// <summary>
        /// Get a franchise by ID.
        /// </summary>
        public async Task<Franchise> GetByIdAsync(int id)
        {
            return await _dbContext.Franchises.FindAsync(id);
        }

        /// <summary>
        /// Update an existing franchise.
        /// </summary>
        public async Task<Franchise> UpdateAsync(Franchise obj)
        {
            _dbContext.Entry(obj).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        /// <summary>
        /// Get characters belonging to a franchise by franchise ID.
        /// </summary>
        public async Task<ICollection<Character>> GetFranchiseCharactersAsync(int franchiseId)
        {
            var franchise = await _dbContext.Franchises
                .Include(f => f.Movies)
                .ThenInclude(m => m.Characters)
                .FirstOrDefaultAsync(f => f.Id == franchiseId);

            if (franchise != null)
            {
                var characters = franchise.Movies.SelectMany(m => m.Characters).ToList();
                return characters;
            }

            return null;
        }

        /// <summary>
        /// Get movies belonging to a franchise by franchise ID.
        /// </summary>
        public async Task<ICollection<Movie>> GetFranchiseMoviesAsync(int franchiseId)
        {
            var franchise = await _dbContext.Franchises
                .Include(f => f.Movies)
                .FirstOrDefaultAsync(f => f.Id == franchiseId);

            if (franchise != null)
            {
                return franchise.Movies.ToList();
            }

            return null;
        }

        /// <summary>
        /// Update the list of movies associated with a franchise.
        /// </summary>
        public async Task UpdateMoviesAsync(ICollection<int> movieIds, int franchiseId)
        {
            var franchise = await _dbContext.Franchises.FindAsync(franchiseId);
            if (franchise != null)
            {
                franchise.Movies.Clear();
                var moviesToAdd = await _dbContext.Movies.Where(m => movieIds.Contains(m.Id)).ToListAsync();
                foreach (var movie in moviesToAdd)
                {
                    franchise.Movies.Add(movie);
                }
                await _dbContext.SaveChangesAsync();
            }
        }

        public Task<Franchise> GetFranchiseByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
