using AutoMapper;
using FilmAPI.Data.Exceptions;
using FilmAPI.Data.Models;
using FilmAPI.Services.Characters;
using Microsoft.EntityFrameworkCore;

namespace FilmAPI.Services.Movies
{
    /// <summary>
    /// Service class for managing movie-related operations.
    /// </summary>
    public class MovieService : IMovieService
    {
        private readonly MoviesDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICharacterService _characterService;

        public MovieService(MoviesDbContext dbContext, IMapper mapper, ICharacterService characterService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _characterService = characterService;
            _characterService = characterService;
        }



        public async Task<Movie> AddAsync(Movie obj)
        {
            await _dbContext.Movies.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }


        public async Task<Movie> UpdateAsync(Movie obj)
        {

            // Check if a movie with the specified Id exists.
            var existingMovie = await _dbContext.Movies.FindAsync(obj.Id);
            if (existingMovie == null)
            {
                // If the movie is not found, throw a custom exception.
                throw new MovieNotFound(obj.Id);
            }

            // Update the existing movie entity with data from the input object.
            _dbContext.Entry(existingMovie).CurrentValues.SetValues(obj);
            await _dbContext.SaveChangesAsync();

            return existingMovie;


        }

        public async Task DeleteAsync(int id)
        {
            var movieToDelete = await _dbContext.Movies.FindAsync(id);
            if (movieToDelete != null)
            {
                // Remove the movie entity
                _dbContext.Movies.Remove(movieToDelete);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new MovieNotFound(id);
            }
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _dbContext.Movies
                .Include(m => m.Characters)
                .Include(m => m.Franchise)
                .ToListAsync();
        }


        public async Task<Movie> GetByIdAsync(int id)
        {
            var movie = await _dbContext.Movies
                .Where(m => m.Id == id)
                .Include(m => m.Characters)
                .Include(m => m.Franchise)
                .FirstOrDefaultAsync();
            if (movie == null)
            {
                throw new MovieNotFound(id);
            }
            return movie;
        }

        //GetCharactersInMovie: 
        //    A method to retrieve all characters appearing in a specific movie.
        //    It should accept the movie's ID as a parameter and 
        //    return a collection of character entities associated with that movie.

        public async Task<IEnumerable<int>> GetCharacterIdsInMovieAsync(int movieId)
        {
            var movie = await _dbContext.Movies
                .Include(m => m.Characters)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                throw new MovieNotFound(movieId);
            }

            return movie.Characters.Select(c => c.Id);
        }


        public async Task UpdateMovieCharacterAsync(int movieId, List<int> characterIds)
        {
            try
            {
                // Retrieve the movie by 'movieId' and include its associated characters
                var movie = await _dbContext.Movies
                    .Include(m => m.Characters)
                    .FirstOrDefaultAsync(m => m.Id == movieId);

                if (movie != null)
                {
                    movie.Characters.Clear(); // Clear the movie's character collection

                    // Iterate through the 'characterIds' and add corresponding characters to the movie.
                    foreach (var characterId in characterIds)
                    {
                        var character = await _characterService.GetByIdAsync(characterId);
                        if (character == null)
                            throw new CharacterNotFound(characterId);

                        // Add the character to the movie's character collection
                        movie.Characters.Add(character);
                    }

                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new MovieNotFound(movieId);
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions and rethrow them as custom exceptions if needed.
                throw new Exception("An error occurred while updating movie characters.", ex);
            }
        }



    }
}