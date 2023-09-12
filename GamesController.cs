using Apricode;
using Microsoft.EntityFrameworkCore;

class GamesController
{
    private IQueryable<GameBd> Games(Context db)
    {
        return db.Games;
    }

    private IQueryable<GenreToGame> Genres(Context db)
    {
        return db.Genres;
    }


    private void AddGames(Context db)
    {
        db.Add(db.Games);
        db.SaveChanges();
    }

    public Game[] Get(GenreValue? genre, Context db)
    {
        var query = Games(db)
            .Join(Genres(db),
                x => x.Id,
                x => x.IdGame,
                (x, y) => new {x, y});
        if (genre != null)
            query = query
                .Where(x => x.y.value == genre);
        return query
            .ToArray()
            .GroupBy(x => x.x.Id)
            .Select(x =>
            {
                var game = x.First(y => y.x.Id == x.Key).x;
                var genres = x.Select(y => y.y).ToArray();

                return new Game
                {
                    Creator = game.Creator,
                    Name = game.Name,
                    Genres = genres
                };
            })
            .ToArray();
    }

    public void Add(Game game, Context db)
    {
        Console.WriteLine("CALL Add");
        if (game == null)
            throw new InvalidOperationException("Нету объекта");
        db.Add(game);
        db.SaveChanges();
    }

    public void Change(Game game, Context db)
    {
        Console.WriteLine("CALL Change");
        if (game == null)
            throw new InvalidOperationException("Нету объекта");
        var oldGame = Games(db).Single(x => x.Id == game.Id);
        oldGame.Name = game.Name;
        oldGame.Creator = game.Creator;
        db.SaveChanges();
        
        db.Genres.RemoveRange(Genres(db)
            .Where(x => x.IdGame == game.Id));
        db.SaveChanges();
        
        db.Genres.AddRange(game.Genres);
        db.SaveChanges();
    }

    public void Del(Game game, Context db)
    {
        Console.WriteLine("CALL Del");
        if (game == null)
            throw new InvalidOperationException("Нету объекта");
        
        db.Genres.RemoveRange(Genres(db)
            .Where(x => x.IdGame == game.Id));
        db.SaveChanges();
        
        var oldGame = Games(db).Single(x => x.Id == game.Id);
        
        db.Games.Remove(oldGame);
        db.SaveChanges();
    }
}

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Creator { get; set; }
    public GenreToGame[] Genres { get; set; }
}

public class GameBd
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Creator { get; set; }
}

public class GenreToGame
{
    public int Id { get; set; }
    public int IdGame { get; set; }
    public GenreValue value { get; set; }
}

public enum GenreValue
{
    Rpg,
    Strategy3
}