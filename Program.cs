using FilmAPI.Data.Models;
using FilmAPI.Services.Characters;
using FilmAPI.Services.Franchises;
using FilmAPI.Services.Movies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//Configure Services
builder.Services.AddDbContext<MoviesDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MoviesDb"));
});

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<IFranchiseService, FranchiseService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
