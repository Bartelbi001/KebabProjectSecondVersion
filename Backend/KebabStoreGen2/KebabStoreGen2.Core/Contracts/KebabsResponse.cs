namespace KebabStoreGen2.Core.Contracts;

public record KebabsResponse(
    Guid id,
    string KebabName,
    string KebabDescription,
    decimal Price,
    string TitleImagePath);

