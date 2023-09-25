using FilmAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace FilmAPI.Services.Movies
{

    /// <summary>
    /// Service for managing movie data.
    /// </summary>
    public class MovieService : IMovieService
    {
        private readonly MoviesDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context used for interaction with the database.</param>
        public MovieService(MoviesDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        // Add a new movie
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


        //Delete a movie by its ID
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

        //Retrieve all movies
        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _dbContext.Movies.ToListAsync();
        }

        //Retrieve a movie by its ID
        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _dbContext.Movies.FindAsync(id);
        }


        //Retrieve the characters associated with a movie by its ID
        public async Task<ICollection<Character>> GetMovieCharactersAsync(int movieId)
        {
            return await _dbContext.Movies
                .Where(m => m.Id == movieId)
                .SelectMany(m => m.Characters)
                .ToListAsync();
        }


        //Update an existing movie
        public async Task<Movie> UpdateAsync(Movie obj)
        {
            _dbContext.Entry(obj).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return obj;
        }


        // Update the characters associated with a movie
        public async Task UpdateCharactersAsync(ICollection<int> characterIds, int movieId)
        {
            var movie = await _dbContext.Movies
            .Include(m => m.Characters)
            .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                // Clear existing character associations
                movie.Characters.Clear();

                //Add new character associations
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
