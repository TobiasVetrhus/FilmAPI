using System.ComponentModel.DataAnnotations;

namespace FilmAPI.Data.Models
{
    /// <summary>
    /// Represents a movie entity in the database.
    /// </summary>

    public class Movie
    {
        public int Id { get; set; }
        //[Required]
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(150)]
        public string Genre { get; set; }

        //[MaxLength(4)]
        public int ReleaseYear { get; set; }
        [StringLength(50)]
        public string Director { get; set; }
        [StringLength(2048)]
        public string Picture { get; set; }
        [StringLength(2048)]
        public string Trailer { get; set; }
        public int FranchiseId { get; set; }


        /// <summary>
        /// Gets or sets the associated franchise (navigation property).
        /// </summary>
        /// <remarks>
        /// This represents a One-to-Many (1-M) relationship between a movie and a franchise,
        /// where a movie belongs to one franchise, but a franchise can have multiple associated movies.
        /// </remarks>
        public Franchise Franchise { get; set; }


        /// <summary>
        /// Gets or sets the collection of characters associated with the movie (navigation property).
        /// </summary>
        /// <remarks>
        /// This represents a Many-to-Many (M-M) relationship between movies and characters,
        /// where a movie can have multiple associated characters, and a character can appear in multiple movies.
        /// </remarks>
        public ICollection<Character>? Characters { get; set; }
    }
}
