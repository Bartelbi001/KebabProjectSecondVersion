using CSharpFunctionalExtensions;
using KebabStoreGen2.Core.Constants;

namespace KebabStoreGen2.Core.Models;

public class Ingredient
{
    private Ingredient(Guid id, string ingredientName, decimal caloriesPer100g, decimal proteinPer100g,
        decimal fatPer100g, decimal carbsPer100g, decimal sugarPer100g, bool containsLactose)
    {
        Id = id;
        IngredientName = ingredientName;
        CaloriesPer100g = caloriesPer100g;
        ProteinPer100g = proteinPer100g;
        FatPer100g = fatPer100g;
        CarbsPer100g = carbsPer100g;
        SugarPer100g = sugarPer100g;
        ContainsLactose = containsLactose;
    }

    public Guid Id { get; }
    public string IngredientName { get; private set; } = string.Empty;
    public decimal CaloriesPer100g { get; private set; }
    public decimal ProteinPer100g { get; private set; }
    public decimal FatPer100g { get; private set; }
    public decimal CarbsPer100g { get; private set; }
    public decimal SugarPer100g { get; private set; }
    public bool ContainsLactose { get; private set; }

    public static Result<Ingredient> Create(Guid id, string ingredientName, decimal caloriesPer100g, decimal proteinPer100g, decimal fatPer100g,
        decimal carbsPer100g, decimal sugarPer100g, bool containsLactose)
    {
        if (string.IsNullOrWhiteSpace(ingredientName) || ingredientName.Length > IngredientConstants.MAX_INGREDIENTNAME_LENGTH)
        {
            return Result.Failure<Ingredient>($"'{nameof(ingredientName)}' is required and must be shorter or equal than {IngredientConstants.MAX_INGREDIENTNAME_LENGTH} characters");
        }

        if (caloriesPer100g < 0 || proteinPer100g < 0 || fatPer100g < 0 || carbsPer100g < 0 || sugarPer100g < 0)
        {
            return Result.Failure<Ingredient>($"Nutritional values can't be negative");
        }

        return Result.Success(new Ingredient(id, ingredientName, caloriesPer100g, proteinPer100g, fatPer100g, carbsPer100g, sugarPer100g, containsLactose));
    }
}