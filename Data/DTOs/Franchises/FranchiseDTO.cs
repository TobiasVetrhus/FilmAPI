using FilmAPI.Data.DTOs.Movies;
using FilmAPI.Data.DTOs.Characters;
using System.Collections.Generic;

namespace FilmAPI.Data.DTOs.Franchises
{
    public class FranchiseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<MovieDto> Movies { get; set; }
        public IEnumerable<CharacterDTO> Characters { get; set; }
    }
}