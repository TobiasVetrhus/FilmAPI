using AutoMapper;
using FilmAPI.Data.DTOs.Characters;
using FilmAPI.Data.Models;

namespace FilmAPI.Mappers
{
    public class CharacterProfile : Profile
    {
        public CharacterProfile()
        {
            CreateMap<CharacterPostDTO, Character>().ReverseMap();
            CreateMap<CharacterDTO, Character>().ReverseMap();
        }
    }
}
