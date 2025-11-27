namespace GameStore.Dtos;

public record class UpdateGameDto(
    string Name,
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate
);