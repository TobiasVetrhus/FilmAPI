using AutoMapper;
using FilmAPI.Data.DTOs.Franchises;
using FilmAPI.Data.Models;

namespace FilmAPI.Mappers
{
    public class FranchiseProfile : Profile
    {
        public FranchiseProfile()
        {
            CreateMap<Franchise, FranchisePostDTO>().ReverseMap();
            CreateMap<Franchise, FranchisePutDTO>().ReverseMap();

            CreateMap<Franchise, FranchiseDTO>()
                .ForMember(fdto => fdto.Movies, options => options.MapFrom(f => f.Movies.Select(m => m.Id).ToArray()));

        }
    }
}
