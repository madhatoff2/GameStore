using Microsoft.EntityFrameworkCore;

namespace GameStore.DataAccess.Models
{
    public class GameStoreContext : DbContext
    {
        public GameStoreContext (DbContextOptions<GameStoreContext> options) : base(options)
        {
        }

        public DbSet<Game> Game { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Platform> Platform { get; set; }
        public DbSet<GameGenre> GameGenre { get; set; }
        public DbSet<GamePlatform> GamePlatform { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameGenre>()
                .HasKey(CompKey => new { CompKey.GameId, CompKey.GenreId });

            modelBuilder.Entity<GamePlatform>()
                .HasKey(CompKey => new { CompKey.GameId, CompKey.Platform });
        }
    }
}
