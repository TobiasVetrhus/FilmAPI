using System.ComponentModel.DataAnnotations;

namespace FilmAPI.Data.DTOs.Franchises
{
    public class FranchisePutDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(250)]
        public string? Description { get; set; }
    }
}
