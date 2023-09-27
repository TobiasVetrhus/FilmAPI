using System.ComponentModel.DataAnnotations;

namespace FilmAPI.Data.Models
{
    public class Franchise
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }

        // Navigation property to represent the relationship with movies.
        public ICollection<Movie>? Movies { get; set; } // 1-to-Many relationship with movies
    }
}

