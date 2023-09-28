using System.ComponentModel.DataAnnotations;

namespace FilmAPI.Data.DTOs.Movies
{
    public class MovieDtoPost
    {

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


    }
}
