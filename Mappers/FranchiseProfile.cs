using AutoMapper;
using FilmAPI.Data.DTOs.Franchises;
using FilmAPI.Data.Models;

namespace FilmAPI.Mappers
{
    public class FranchiseProfile : Profile
    {
        public FranchiseProfile()
        {
            CreateMap<Franchise, FranchiseDTO>().ReverseMap();
         
        }
    }
}
