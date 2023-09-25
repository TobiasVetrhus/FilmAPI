namespace FilmAPI.Data.Exceptions
{
    /// <summary>
    /// Exception class for representing a specific entity not found in the database.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specific entity type and identifier.
        /// </summary>
        /// <param name="entityType">The type of the entity that was not found.</param>
        /// <param name="entityIdentifier">The identifier or name of the entity that was not found.</param>
        public EntityNotFoundException(string entityType, object entityIdentifier)
            : base($"Entity '{entityType}' with identifier '{entityIdentifier}' not found.")
        { }
    }
}
