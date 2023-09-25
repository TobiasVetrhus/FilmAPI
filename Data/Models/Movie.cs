using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmAPI.Data.Models
{
    [Table(nameof(Movie))]
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(150)]
        public string Genre { get; set; }

        [MaxLength(4)]
        public int ReleaseYear { get; set; }
        [StringLength(50)]
        public string Director { get; set; }
        [StringLength(2048)]
        public string Picture { get; set; }
        [StringLength(2048)]
        public string Trailer { get; set; }
        public int? FranchiseId { get; set; }

        //Navigation
        public Franchise Franchise { get; set; } //1-M 
        public ICollection<Character> Characters { get; set; } //M-M
    }
}
