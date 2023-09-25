namespace FilmAPI.Data.Exceptions
{
    public class MovieNotFoundException : EntityNotFoundException
    {
        public MovieNotFoundException(int id)
            : base("Movie", id)
        { }

        public MovieNotFoundException(string name)
            : base("Movie", name)
        { }
    }
}
