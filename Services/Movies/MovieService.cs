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


        /// <summary>
        /// Initializes a new instance of the <see cref="MovieService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="characterService">The character service.</param>
        public MovieService(MoviesDbContext dbContext, IMapper mapper, ICharacterService characterService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _characterService = characterService;
        }


        /// <summary>
        /// Retrieves a list of all movies from the database.
        /// </summary>
        /// <returns>A list of movie objects.</returns>
        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _dbContext.Movies
                .Include(m => m.Characters)
                .Include(m => m.Franchise)
                .ToListAsync();
        }


        /// <summary>
        /// Retrieves a movie by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve.</param>
        /// <returns>The movie object if found, otherwise throws a <see cref="MovieNotFound"/> exception.</returns>
        public async Task<Movie> GetByIdAsync(int id)
        {
            var existingMovie = await _dbContext.Movies
                .Include(m => m.Characters)
                .Include(m => m.Franchise)
                .SingleOrDefaultAsync(m => m.Id == id)
                ?? throw new MovieNotFound(id);
            return existingMovie;
        }


        /// <summary>
        /// Adds a new movie to the database.
        /// </summary>
        /// <param name="obj">The movie object to add.</param>
        /// <returns>The added movie object.</returns>
        public async Task<Movie> AddAsync(Movie obj)
        {
            if (!await _dbContext.Franchises.AnyAsync(f => f.Id == obj.FranchiseId.Value))
            {
                throw new FranchiseNotFound(obj.FranchiseId.Value);
            }
            await _dbContext.Movies.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }


        /// <summary>
        /// Updates an existing movie in the database.
        /// </summary>
        /// <param name="obj">The movie object with updated information.</param>
        /// <returns>The updated movie object.</returns>
        public async Task<Movie> UpdateAsync(Movie obj)
        {
            var existingMovie = await _dbContext.Movies.FindAsync(obj.Id) ?? throw new MovieNotFound(obj.Id);
            if (!await _dbContext.Franchises.AnyAsync(f => f.Id == obj.FranchiseId.Value))
            {
                throw new FranchiseNotFound(obj.FranchiseId.Value);
            }
            _dbContext.Entry(existingMovie).CurrentValues.SetValues(obj);
            await _dbContext.SaveChangesAsync();
            return existingMovie;
        }



        /// <summary>
        /// Deletes a movie from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        public async Task DeleteAsync(int id)
        {
            var movieToDelete = await _dbContext.Movies.FindAsync(id) ?? throw new MovieNotFound(id);
            _dbContext.Movies.Remove(movieToDelete);
            await _dbContext.SaveChangesAsync();

        }


        /// <summary>
        /// Retrieves character IDs associated with a movie from the database.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <returns>A list of character IDs.</returns>
        public async Task<IEnumerable<int>> GetCharacterIdsInMovieAsync(int movieId)
        {
            var movie = await _dbContext.Movies
                .Include(m => m.Characters)
                .FirstOrDefaultAsync(m => m.Id == movieId) ?? throw new MovieNotFound(movieId);

            return movie.Characters.Select(c => c.Id);
        }


        /// <summary>
        /// Updates the characters associated with a movie in the database.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <param name="characterIds">A list of character IDs to associate with the movie.</param>
        public async Task UpdateMovieCharacterAsync(int movieId, List<int> characterIds)
        {
            var movie = await _dbContext.Movies
                .Include(m => m.Characters)
                .FirstOrDefaultAsync(m => m.Id == movieId) ?? throw new MovieNotFound(movieId);

            movie.Characters.Clear();
            foreach (var characterId in characterIds)
            {
                var character = await _characterService.GetByIdAsync(characterId) ?? throw new CharacterNotFound(characterId);
                movie.Characters.Add(character);
            }
            await _dbContext.SaveChangesAsync();
        }

    }
}