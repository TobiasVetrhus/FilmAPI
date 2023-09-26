using AutoMapper;
using FilmAPI.Data.DTOs.Characters;
using FilmAPI.Data.Models;

namespace FilmAPI.Mappers
{
    public class CharacterProfile : Profile
    {
        public CharacterProfile()
        {
            CreateMap<Character, CharacterPostDTO>().ReverseMap();
            CreateMap<Character, CharacterPutDTO>().ReverseMap();
            CreateMap<Character, CharacterDTO>()
                .ForMember(cdto => cdto.Movies, options => options.MapFrom(c => c.Movies.Select(m => m.Id).ToArray()));

        }
    }
}
