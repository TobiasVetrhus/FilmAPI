using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmAPI.Data.Models
{
    [Table(nameof(Character))]
    public class Character
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string? Alias { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        [StringLength(2048)]
        public string? Picture { get; set; }

        //Navigation
        public ICollection<Movie>? Movies { get; set; } // M-M

    }
}
