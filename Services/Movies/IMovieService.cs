using FilmAPI.Data.Models;

namespace FilmAPI.Services.Movies
{
    /// <summary>
    /// Service interface for managing movie-related operations.
    /// </summary>
    public interface IMovieService : ICrudService<Movie, int>
    {
        /// <summary>
        /// Retrieves character IDs associated with a movie from the database.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <returns>A list of character IDs.</returns>
        Task<IEnumerable<int>> GetCharacterIdsInMovieAsync(int movieId);


        /// <summary>
        /// Updates the characters associated with a movie in the database.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <param name="characterIds">A list of character IDs to associate with the movie.</param>
        Task UpdateMovieCharacterAsync(int movieId, List<int> characterIds);
    }
}
