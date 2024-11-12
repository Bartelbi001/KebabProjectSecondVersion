using KebabStoreGen2.Core.Models.Enums;

namespace KebabStoreGen2.Core.Contracts;

public record KebabResponse(
    Guid id,
    string KebabName,
    string KebabDescription,
    decimal KebabPrice,
    StuffingCategory Stuffing,
    WrapCategory Wrap,
    bool IsAvailable,
    string TitleImagePath,
    List<IngredientResponse> Ingredients,
    int Calories);