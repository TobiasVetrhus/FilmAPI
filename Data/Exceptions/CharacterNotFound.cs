namespace FilmAPI.Data.Exceptions
{
    public class CharacterNotFound : EntityNotFoundException
    {
        public CharacterNotFound(int id)
            : base("Character", id)
        { }

        public CharacterNotFound(string name)
            : base("Character", name)
        { }
    }
}
