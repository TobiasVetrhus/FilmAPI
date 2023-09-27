namespace FilmAPI.Data.DTOs.Movies
{
    /// <summary>
    /// Data transfer object (DTO) for representing movie information.
    /// </summary>
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }
        public string Director { get; set; }
        public string Picture { get; set; }
        public string Trailer { get; set; }
        public int Franchise { get; set; }
        public int[] Characters { get; set; }


    }
}
