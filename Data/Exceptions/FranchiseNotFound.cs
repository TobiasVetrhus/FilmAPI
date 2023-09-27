using System.Net;

namespace FilmAPI.Data.Exceptions
{
    /// <summary>
    /// Exception class for representing a franchise entity not found in the database.
    /// </summary>
    public class FranchiseNotFound : EntityNotFoundException
    {
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FranchiseNotFound"/> class with a specific franchise ID.
        /// </summary>
        /// <param name="id">The ID of the franchise that was not found.</param>
        public FranchiseNotFound(int id)
            : base("Franchise", id, "FRANCHISE_NOT_FOUND")
        {
            StatusCode = HttpStatusCode.NotFound;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FranchiseNotFound"/> class with a specific franchise name.
        /// </summary>
        /// <param name="name">The name of the franchise that was not found.</param>
        public FranchiseNotFound(string name)
            : base("Franchise", name, "FRANCHISE_NOT_FOUND")
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}
