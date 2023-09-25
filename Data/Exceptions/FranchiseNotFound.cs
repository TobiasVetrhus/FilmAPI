namespace FilmAPI.Data.Exceptions
{
    public class FranchiseNotFound : EntityNotFoundException
    {
        public FranchiseNotFoundException(int id)
            : base("Franchise", id)
        { }

        public FranchiseNotFoundException(string name)
            : base("Franchise", name)
        { }
    }
}
