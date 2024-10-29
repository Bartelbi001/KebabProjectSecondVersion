using KebabStoreGen2.Core.Models;

namespace KebabStoreGen2.Core.Abstractions;

public interface INutritionCalculatorService
{
    int CalculateTotalCalories(List<Ingredient> ingredients);
}