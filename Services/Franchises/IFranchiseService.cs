﻿using FilmAPI.Data.Models;

namespace FilmAPI.Services.Franchises
{
    public interface IFranchiseService : ICrudService<Franchise, int>
    {
        Task UpdateMoviesAsync(int franchiseId, int[] movieIds);
        Task<ICollection<Movie>> GetFranchiseMoviesAsync(int franchiseId);
        Task<ICollection<Character>> GetFranchiseCharactersAsync(int franchiseId);
        Task<Franchise> GetFranchiseByIdAsync(int id); // Corrected return type
    }
}