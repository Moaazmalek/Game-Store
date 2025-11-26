namespace GameStore.EndPoints;

using GameStore.Data;
using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Mapping;

public static class GamesEndpoints
{
    private static readonly List<GameSummeryDto> games = [
     new(
        1,
        "Street Fighter II",
        "Fighting",
        19.99m,
        new DateOnly(1991, 6, 2)
    ),
    new(
        2,
        "The Legend of Zelda: Ocarina of Time",
        "Action-Adventure",
        29.99m,
        new DateOnly(1998, 11, 21)
    ),
    new(
        3,
        "Final Fantasy VII",
        "Role-Playing",
        39.99m,
        new DateOnly(1997, 1, 31)
    ),
    new(
        4,
        "Half-Life",
        "First-Person Shooter",
        9.99m,
        new DateOnly(1998, 11, 19)
    ),
    new(
        5,
        "Minecraft",
        "Sandbox",
        26.95m,
        new DateOnly(2011, 11, 18)
    )
 ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var gamesGroup = app.MapGroup("/games");
        // GET / games
        gamesGroup.MapGet("/", () => games);

        //GET /games/{id}
        gamesGroup.MapGet("/{id}", (int id,GameStoreContext dbContext) =>
        {
            var game=dbContext.Games.Find(id);
            return game is not null ? Results.Ok(game) : Results.NotFound();
        });

        //POST /games
        gamesGroup.MapPost("/", (CreateGameDto newGame,GameStoreContext dbContext) =>
        {
            Game game= newGame.ToEntity();            
            dbContext.Games.Add(game);
            dbContext.SaveChanges();

          
          
            return Results.Created($"/games/{game.Id}", game.ToGameDetailsDto());
        }).
        WithParameterValidation();

        // PUT /games/{id}

        gamesGroup.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var gameIndex = games.FindIndex(g => g.Id == id);
            if (gameIndex == -1)
            {
                return Results.NotFound();
            }
            var updatedGameDto = new GameSummeryDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );
            games[gameIndex] = updatedGameDto;
            return Results.Ok(updatedGameDto);
        });

        // DELETE /games/{id}
        gamesGroup.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(g => g.Id == id);
            return Results.NoContent();
        });
        return gamesGroup;
    }

}