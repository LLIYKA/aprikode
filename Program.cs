using Apricode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connection));

var app = builder.Build();

var contro = new GamesController();

app.MapGet("/", ([FromQuery] GenreValue? genre, Context db) => contro.Get(genre, db));
app.MapPost("/add", ([FromBody] Game game, Context db) => contro.Add(game, db));
app.MapPost("/change", ([FromBody] Game game, Context db) => contro.Change(game, db));
app.MapPost("/del", ([FromBody] Game game, Context db) => contro.Del(game, db));

app.Run();