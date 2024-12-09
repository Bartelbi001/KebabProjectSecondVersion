namespace KebabStoreGen2.Core.Contracts;

public record IngredientResponse(
    Guid id,
    string IngredientName,
    decimal CaloriesPer100g,
    decimal ProteinPer100g,
    decimal FatPer100g,
    decimal CarbsPer100g,
    decimal? SugarPer100g = null,
    bool? ContainsLactose = null
);