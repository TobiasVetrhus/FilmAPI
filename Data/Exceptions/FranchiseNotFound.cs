namespace FilmAPI.Data.Exceptions
{
    public class FranchiseNotFound : EntityNotFoundException
    {
        public FranchiseNotFound(int id)
            : base("Franchise", id)
        { }

        public FranchiseNotFound(string name)
            : base("Franchise", name)
        { }
    }
}
