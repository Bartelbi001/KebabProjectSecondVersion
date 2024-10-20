namespace KebabStoreGen2.Core.Contracts;

public record KebabsResponse(
    Guid id,
    string Name,
    string Description,
    decimal Price,
    string TitleImagePath);

