using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Models;

namespace KebabStoreGen2.Application.Services;

public class CalculateTotalNutritionsService : NutritionAndWeightCalculatorService
{
    public int CalculateTotalWeight(List<int> weights)
    {
        return weights.Sum();
    }
    public decimal CalculateTotalCalories(List<Ingredient> ingredients, List<int> weights)
    {
        return ingredients.Zip(weights, (ingredient, weight) => (ingredient.CaloriesPer100g * weight) / 100).Sum();
    }

    public decimal CalculateTotalCarbs(List<Ingredient> ingredients, List<int> weights)
    {
        return ingredients.Zip(weights, (ingredient, weight) => (ingredient.CarbsPer100g * weight) / 100).Sum();
    }

    public decimal CalculateTotalFat(List<Ingredient> ingredients, List<int> weights)
    {
        return ingredients.Zip(weights, (ingredient, weight) => (ingredient.FatPer100g * weight) / 100).Sum();
    }

    public decimal CalculateTotalProtein(List<Ingredient> ingredients, List<int> weights)
    {
        return ingredients.Zip(weights, (ingredient, weight) => (ingredient.ProteinPer100g * weight) / 100).Sum();
    }

    public decimal CalculateTotalSugar(List<Ingredient> ingredients, List<int> weights)
    {
        return ingredients.Zip(weights, (ingredient, weight) => ((ingredient.SugarPer100g ?? 0) * weight) / 100).Sum();
    }
}