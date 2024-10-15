namespace KebabStoreGen2.API.Contracts;

public record KebabsResponse(
    Guid id,
    string Name,
    string Description,
    decimal Price,
    string TitleImagePath);

