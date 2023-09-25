using System.ComponentModel.DataAnnotations;

namespace FilmAPI.Data.DTOs.Characters
{
    public class CharacterPutDTO
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
    }
}
