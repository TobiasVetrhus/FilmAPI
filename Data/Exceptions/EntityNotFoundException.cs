namespace FilmAPI.Data.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entity, int id)
            : base($"{entity} does not exist with ID: {id}")
        { }

        public EntityNotFoundException(string entity, string name)
            : base($"{entity} does not exist with name: {name}")
        { }
    }
}
