using AutoMapper;
using FilmAPI.Data.DTOs.Characters;
using FilmAPI.Data.Models;

namespace FilmAPI.Mappers
{
    // AutoMapper profile for mapping between Character entities and related DTOs.
    public class CharacterProfile : Profile
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterProfile"/> class.
        /// Configures mappings for Character to CharacterDTO, CharacterPostDTO and CharacterPutDTO.
        /// </summary>
        public CharacterProfile()
        {
            CreateMap<Character, CharacterPostDTO>().ReverseMap();
            CreateMap<Character, CharacterPutDTO>().ReverseMap();
            CreateMap<Character, CharacterDTO>()
                .ForMember(cdto => cdto.MovieIds, options => options.MapFrom(c => c.Movies.Select(m => m.Id).ToArray())); //Map from the 'Movies' navigation property of the 'Character' model to an array of 'MovieIds'.

        }
    }
}
