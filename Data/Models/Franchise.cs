using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmAPI.Data.Models
{
    public class Franchise
    {
        public int Id { get; set; }

        /// <summary>
        /// The name of the franchise.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// The description of the franchise.
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        // Navigation property to represent the relationship with movies.
        public ICollection<Movie> Movies { get; set; } // 1-to-Many relationship with movies
    }
}

