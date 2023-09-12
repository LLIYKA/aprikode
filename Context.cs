using Microsoft.EntityFrameworkCore;
namespace Apricode;

public class Context: DbContext
{
    public DbSet<GameBd> Games { get; set; } = null!;
    public DbSet<GenreToGame> Genres { get; set; } = null!;
    public Context(DbContextOptions<Context> options)
        : base(options)
    {
        Database.EnsureCreated();   // создаем базу данных при первом обращении
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>().HasData(
            new Game
            {
                Id = 1,
                Name = "STRIKER",
                Creator = "1C",
                
            },
            new Game
            {
                Id = 2,
                Name = "STRIKER2",
                Creator = "2C",
               
            },
            new Game
            {
                Id = 3,
                Name = "STRIKER3",
                Creator = "3C",
               
            }
        );
    }
}
