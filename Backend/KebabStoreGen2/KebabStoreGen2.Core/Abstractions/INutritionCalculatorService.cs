using KebabStoreGen2.Core.Models;

namespace KebabStoreGen2.Core.Abstractions;

public interface NutritionAndWeightCalculatorService
{
    int CalculateTotalWeight(List<int> weights);
    decimal CalculateTotalCalories(List<Ingredient> ingredients, List<int> weights);
    decimal CalculateTotalProtein(List<Ingredient> ingredients, List<int> weights);
    decimal CalculateTotalFat(List<Ingredient> ingredients, List<int> weights);
    decimal CalculateTotalCarbs(List<Ingredient> ingredients, List<int> weights);
    decimal CalculateTotalSugar(List<Ingredient> ingredients, List<int> weights);
}