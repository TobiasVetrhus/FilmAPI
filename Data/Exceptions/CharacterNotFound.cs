namespace FilmAPI.Data.Exceptions
{
    public class CharacterNotFound : EntityNotFoundException
    {
        public CharacterNotFoundException(int id)
            : base("Character", id)
        { }

        public CharacterNotFoundException(string name)
            : base("Character", name)
        { }
    }
}
