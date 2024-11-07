namespace KebabStoreGen2.Core.Contracts;

public record IngredientResponse(
    Guid id,
    string IngredientName,
    int WeightInGrams,
    int Calories,
    int Protein,
    int Fat,
    int Carbs);