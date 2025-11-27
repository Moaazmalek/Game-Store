namespace GameStore.EndPoints;

using GameStore.Data;
using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;

public static class GamesEndpoints
{
  

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var gamesGroup = app.MapGroup("/games");
        // GET / games
        gamesGroup.MapGet("/",async (GameStoreContext dbContext) => 
            await dbContext.Games.
            Include(g=>g.Genre).
            Select((game) => game.ToGameSummaryDto())
            .AsNoTracking() 
            .ToListAsync()
        );

        //GET /games/{id}
        gamesGroup.MapGet("/{id}",async (int id,GameStoreContext dbContext) =>
        {
            var game=await dbContext.Games.FindAsync(id);
            return game is not null ? Results.Ok(game) : Results.NotFound();
        });

        //POST /games
        gamesGroup.MapPost("/",async (CreateGameDto newGame,GameStoreContext dbContext) =>
        {
            Game game= newGame.ToEntity();            
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
          
          
            return Results.Created($"/games/{game.Id}", game.ToGameDetailsDto());
        }).
        WithParameterValidation();

        // PUT /games/{id}

        gamesGroup.MapPut("/{id}",async (int id, UpdateGameDto updatedGame,GameStoreContext dbContext) =>
        {
            var existingGame=await dbContext.Games.FindAsync(id);
            if (existingGame is null)
            {
                return Results.NotFound();
            }
            dbContext.Entry(existingGame).CurrentValues.SetValues(updatedGame.ToEntity(id));
            await dbContext.SaveChangesAsync();

            return Results.NoContent();;
        });
    
        // DELETE /games/{id}
       gamesGroup.MapDelete("/{id}",async (int id,GameStoreContext dbContext) =>
       {
           await dbContext.Games.Where(g=>g.Id==id).ExecuteDeleteAsync();
           return Results.NoContent();
       });
        return gamesGroup;
    }

}