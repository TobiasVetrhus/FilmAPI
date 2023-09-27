using System.Net;

namespace FilmAPI.Data.Exceptions
{/// <summary>
 /// Exception class for representing a movie entity not found in the database.
 /// </summary>
    public class MovieNotFound : EntityNotFoundException
    {
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieNotFoundException"/> class with a specific movie ID.
        /// </summary>
        /// <param name="id">The ID of the movie that was not found.</param>
        public MovieNotFound(int id)
            : base("Movie", id, "MOVIE_NOT_FOUND")
        {
            StatusCode = HttpStatusCode.NotFound;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieNotFoundException"/> class with a specific movie name.
        /// </summary>
        /// <param name="name">The name of the movie that was not found.</param>
        public MovieNotFound(string name)
            : base("Movie", name, "MOVIE_NOT_FOUND")
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}
