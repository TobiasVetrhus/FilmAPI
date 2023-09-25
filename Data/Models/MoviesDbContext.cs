using Microsoft.EntityFrameworkCore;

namespace FilmAPI.Data.Models
{
    public class MoviesDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public MoviesDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Franchise> Franchises { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Characters
            modelBuilder.Entity<Character>().HasData(
                new Character() { Id = 1, FullName = "Katniss Everdeen", Alias = "The Girl on Fire", Gender = "Female" },
                new Character() { Id = 2, FullName = "Peeta Mellark", Gender = "Male" },
                new Character() { Id = 3, FullName = "Anakin Skywalker", Alias = "The Chosen One", Gender = "Male" }
                );

            //Movies
            modelBuilder.Entity<Movie>().HasData(
                new Movie() { Id = 1, Title = "The Hunger Games", Genre = "Action, Adventure, Sci-Fi, Thriller", ReleaseYear = 2012, Director = "Gary Ross", Picture = "https://m.media-amazon.com/images/M/MV5BMjA4NDg3NzYxMF5BMl5BanBnXkFtZTcwNTgyNzkyNw@@._V1_.jpg", Trailer = "https://www.youtube.com/watch?v=PbA63a7H0bo", FranchiseId = 1 },
                new Movie() { Id = 2, Title = "Episode 2 - Attack of the Clones", Genre = "Action, Adventure, Fantasy, Sci-Fi", ReleaseYear = 2002, Director = "George Lucas", Picture = "https://m.media-amazon.com/images/M/MV5BMDAzM2M0Y2UtZjRmZi00MzVlLTg4MjEtOTE3NzU5ZDVlMTU5XkEyXkFqcGdeQXVyNDUyOTg3Njg@._V1_.jpg", Trailer = "https://www.youtube.com/watch?v=gYbW1F_c9eM", FranchiseId = 2 },
                new Movie() { Id = 3, Title = "Episode 3 - Revenge of the Sith", Genre = "Action, Adventure, Fantasy, Sci-Fi", ReleaseYear = 2005, Director = "George Lucas", Picture = "https://m.media-amazon.com/images/M/MV5BNTc4MTc3NTQ5OF5BMl5BanBnXkFtZTcwOTg0NjI4NA@@._V1_.jpg", Trailer = "https://www.youtube.com/watch?v=5UnjrG_N8hU", FranchiseId = 2 }
                );

            //Franchise
            modelBuilder.Entity<Franchise>().HasData(
                new Franchise() { Id = 1, Name = "The Hunger Games film series", Description = "Science fiction dystopian adventure films, based on The Hunger Games trilogy novels by Suzanne Collins." },
                new Franchise() { Id = 2, Name = "Skywalker Saga", Description = "A Star Wars trilogy of subtrilogies; the original trilogy, the prequel trilogy, and the sequel trilogy." }
                );

            //MovieCharacters
            modelBuilder.Entity<Movie>()
                .HasMany(mv => mv.Characters)
                .WithMany(chr => chr.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieCharacters",
                    l => l.HasOne<Character>().WithMany().HasForeignKey("CharacterId"),
                    r => r.HasOne<Movie>().WithMany().HasForeignKey("MovieId"),
                    je =>
                    {
                        je.HasKey("MovieId", "CharacterId");
                        je.HasData(
                            new { MovieId = 1, CharacterId = 1 },
                            new { MovieId = 1, CharacterId = 2 },
                            new { MovieId = 2, CharacterId = 3 },
                            new { MovieId = 3, CharacterId = 3 }
                            );
                    }
                );
        }
    }
}
