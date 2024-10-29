using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Models;

namespace KebabStoreGen2.Application.Services;

public class CalculateTotalCaloriesService : INutritionCalculatorService
{
    public int CalculateTotalCalories(List<Ingredient> ingredients)
    {
        int totalCalories = 0;
        foreach (var ingredient in ingredients)
        {
            totalCalories += ingredient.Calories;
        }

        return totalCalories;
    }
}