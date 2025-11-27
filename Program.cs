using GameStore.Data;
using GameStore.EndPoints;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameStoreContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();


app.MapGamesEndpoints();


// GET /
app.MapGet("/", () => "Hello, World!" );

await app.MigrateDbAsync();

app.Run();
