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
                .ForMember(dest => dest.Characters, opt => opt.MapFrom(src => src.Characters.Select(c => c.Id).ToArray()));
            //.ForMember(dest => dest.FranchiseId, opt => opt.MapFrom(src => src.Franchise.Id));

            //CreateMap<MovieDto, Movie>()
            //    .ForMember(dest => dest.Characters, opt => opt.Ignore()) // Ignore Characters mapping for reverse
            //    .ForMember(dest => dest.Franchise, opt => opt.Ignore()); // Ignore Franchise mapping for reverse
            CreateMap<Movie, MovieDtoPut>().ReverseMap();
            CreateMap<Movie, MovieDtoPost>().ReverseMap();
        }

    }
}
