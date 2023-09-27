using AutoMapper;
using FilmAPI.Data.DTOs.Movies;
using FilmAPI.Data.Models;

namespace FilmAPI.Mappers
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Characters, opt => opt.MapFrom(src => src.Characters.Select(c => c.Id).ToArray()))
                .ForMember(dest => dest.Franchise, opt => opt.MapFrom(src => src.Franchise.Id));
        }
    }
}
