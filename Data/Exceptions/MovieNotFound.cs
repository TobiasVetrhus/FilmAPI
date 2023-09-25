namespace FilmAPI.Data.Exceptions
{
    /// <summary>
    /// Exception class for representing a movie entity not found in the database.
    /// </summary>
    public class MovieNotFoundException : EntityNotFoundException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovieNotFoundException"/> class with a specific movie ID.
        /// </summary>
        /// <param name="id">The ID of the movie that was not found.</param>
        public MovieNotFoundException(int id)
            : base("Movie", id)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieNotFoundException"/> class with a specific movie name.
        /// </summary>
        /// <param name="name">The name of the movie that was not found.</param>
        public MovieNotFoundException(string name)
            : base("Movie", name)
        { }
    }
}
