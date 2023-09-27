using System.Net;

namespace FilmAPI.Data.Exceptions
{
    /// <summary>
    /// Exception class for representing a character entity not found in the database.
    /// </summary>
    public class CharacterNotFound : EntityNotFoundException
    {
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterNotFound"/> class with a specific character ID.
        /// </summary>
        /// <param name="id">The ID of the character that was not found.</param>
        public CharacterNotFound(int id)
            : base("Character", id, "CHARACTER_NOT_FOUND")
        {
            StatusCode = HttpStatusCode.NotFound;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterNotFound"/> class with a specific character name.
        /// </summary>
        /// <param name="name">The name of the character that was not found.</param>
        public CharacterNotFound(string name)
            : base("Character", name, "CHARACTER_NOT_FOUND")
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}
