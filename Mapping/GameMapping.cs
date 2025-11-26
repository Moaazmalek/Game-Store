

using GameStore.Dtos;
using GameStore.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameStore.Mapping;

public static class GameMapping
{
    public static Game ToEntity(this CreateGameDto game)
    {
         return new Game()
            {
                Name=game.Name,
                GenreId=game.GenreId,
                Price=game.Price,
                ReleaseDate=game.ReleaseDate
                
            };
    }
    public static GameSummeryDto ToGameSummaryDto(this Game game)
    {
       return new GameSummeryDto(
                game.Id,
                game.Name,
                game.Genre!.Name,
                game.Price,
                game.ReleaseDate
            );
    }
    public static GameDetailsDto ToGameDetailsDto(this Game game)
    {
       return new GameDetailsDto(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );
    }

}
