namespace FilmAPI.Data.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(int id)
            : base($"Character does not exist with ID: {id}")
        { }

        public EntityNotFoundException(string name)
            : base($"Character does not exist with name: {name}")
        { }
    }
}
