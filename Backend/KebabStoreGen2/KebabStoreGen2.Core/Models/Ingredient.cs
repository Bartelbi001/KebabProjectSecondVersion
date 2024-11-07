using CSharpFunctionalExtensions;
using KebabStoreGen2.Core.Constants;

namespace KebabStoreGen2.Core.Models;

public class Ingredient
{
    private Ingredient(Guid id, string ingredientName, int weightInGrams, int calories, int protein, int fat, int carbs)
    {
        Id = id;
        IngredientName = ingredientName;
        WeightInGrams = weightInGrams;
        Calories = calories;
        Protein = protein;
        Fat = fat;
        Carbs = carbs;
    }

    public Guid Id { get; }
    public string IngredientName { get; private set; } = string.Empty;
    public int WeightInGrams { get; private set; }
    public int Calories { get; private set; }
    public int Protein { get; private set; }
    public int Fat { get; private set; }
    public int Carbs { get; private set; }

    public static Result<Ingredient> Create(Guid id, string ingredientName, int weightInGrams, int calories, int protein, int fat, int carbs)
    {
        if (string.IsNullOrWhiteSpace(ingredientName) || ingredientName.Length > IngredientConstants.MAX_INGREDIENTNAME_LENGTH)
        {
            return Result.Failure<Ingredient>($"'{nameof(ingredientName)}' is required and must be shorter or equal than {IngredientConstants.MAX_INGREDIENTNAME_LENGTH} characters");
        }

        if (weightInGrams < 0)
        {
            return Result.Failure<Ingredient>($"'{nameof(weightInGrams)}' can't be negative");
        }

        if (calories < 0)
        {
            return Result.Failure<Ingredient>($"'{nameof(calories)}' can't be negative");
        }

        if (protein < 0)
        {
            return Result.Failure<Ingredient>($"'{nameof(protein)}' can't be negative");
        }

        if (fat < 0)
        {
            return Result.Failure<Ingredient>($"'{nameof(fat)}' can't be negative");
        }

        if (carbs < 0)
        {
            return Result.Failure<Ingredient>($"'{nameof(carbs)}' can't be negative");
        }

        return Result.Success(new Ingredient(id, ingredientName, weightInGrams, calories, protein, fat, carbs));
    }
}
