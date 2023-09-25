using FilmAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmAPI.Services.Movies
{
    /// <summary>
    /// Service class for managing movie-related operations.
    /// </summary>
    public class MovieService : IMovieService
    {
        private readonly MoviesDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context for movies.</param>
        public MovieService(MoviesDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Adds a new movie to the database.
        /// </summary>
        /// <param name="obj">The movie entity to add.</param>
        /// <returns>The added movie entity.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="obj"/> is null.</exception>
        public async Task<Movie> AddAsync(Movie obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            _dbContext.Movies.Add(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        /// <summary>
        /// Deletes a movie by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        public async Task DeleteAsync(int id)
        {
            var movieToDelete = await _dbContext.Movies.FindAsync(id);
            if (movieToDelete != null)
            {
                // Handle delete in a way that sets related data to null
                movieToDelete.Characters.Clear(); // Clear character associations
                movieToDelete.FranchiseId = null; // Set franchise to null
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Retrieves a list of all movies from the database.
        /// </summary>
        /// <returns>A list of movie entities.</returns>
        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _dbContext.Movies.ToListAsync();
        }

        /// <summary>
        /// Retrieves a movie by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve.</param>
        /// <returns>The movie entity.</returns>
        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _dbContext.Movies.FindAsync(id);
        }

        /// <summary>
        /// Updates an existing movie in the database.
        /// </summary>
        /// <param name="obj">The updated movie entity.</param>
        /// <returns>The updated movie entity.</returns>
        public async Task<Movie> UpdateAsync(Movie obj)
        {
            _dbContext.Entry(obj).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        /// <summary>
        /// Updates the characters associated with a movie in the database.
        /// </summary>
        /// <param name="characterIds">The IDs of characters to associate with the movie.</param>
        /// <param name="movieId">The ID of the movie to update.</param>
        public async Task UpdateCharactersAsync(ICollection<int> characterIds, int movieId)
        {
            var movie = await _dbContext.Movies
                .Include(m => m.Characters)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie != null)
            {
                // Clear existing character associations
                movie.Characters.Clear();

                // Add new character associations
                foreach (var characterId in characterIds)
                {
                    var character = await _dbContext.Characters.FindAsync(characterId);
                    if (character != null)
                    {
                        movie.Characters.Add(character);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
