﻿using CSharpFunctionalExtensions;
using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Constants;
using KebabStoreGen2.Core.Models.Enums;

namespace KebabStoreGen2.Core.Models;

public class Kebab
{
    private Kebab(Guid id, string name, string description, decimal kebabPrice, StuffingCategory stuffing, WrapCategory wrap,
        bool isAvailable, Image? titleImage, List<Ingredient> ingredients, int totalWeight, decimal totalCalories,
        decimal totalCarbs, decimal totalFat, decimal totalProtein)
    {
        Id = id;
        KebabName = name;
        KebabDescription = description;
        KebabPrice = kebabPrice;
        Stuffing = stuffing;
        Wrap = wrap;
        IsAvailable = isAvailable;
        TitleImage = titleImage;
        Ingredients = ingredients;
        TotalWeight = totalWeight;
        TotalCalories = totalCalories;
        TotalCarbs = totalCarbs;
        TotalFat = totalFat;
        TotalProtein = totalProtein;
    }

    public Guid Id { get; }
    public string KebabName { get; private set; } = string.Empty;
    public string KebabDescription { get; private set; } = string.Empty;
    public decimal KebabPrice { get; private set; }
    public StuffingCategory Stuffing { get; private set; }
    public WrapCategory Wrap { get; private set; }
    public bool IsAvailable { get; private set; }
    public Image? TitleImage { get; private set; }
    public List<Ingredient> Ingredients { get; private set; }
    public int TotalWeight { get; private set; }
    public decimal TotalCalories { get; private set; }
    public decimal TotalCarbs { get; private set; }
    public decimal TotalFat { get; private set; }
    public decimal TotalProtein { get; private set; }

    public static Result<Kebab> Create(Guid id, string kebabName, string kebabDescription, decimal kebabPrice,
        StuffingCategory stuffing, WrapCategory wrap, bool isAvailable,
        Image? titleImage, List<Ingredient> ingredients, INutritionAndWeightCalculatorService nutritionAndWeightCalculatorService, List<int> ingredientWeights)
    {
        if (string.IsNullOrWhiteSpace(kebabName) || kebabName.Length > KebabConstants.MAX_KEBABNAME_LENGTH)
        {
            return Result.Failure<Kebab>($"'{nameof(kebabName)}' is required and must be shorter than {KebabConstants.MAX_KEBABNAME_LENGTH}");
        }

        if (string.IsNullOrWhiteSpace(kebabDescription) || kebabDescription.Length > KebabConstants.MAX_KEBABDESCRIPTION_LENGTH)
        {
            return Result.Failure<Kebab>($"'{nameof(kebabDescription)}' is required and must be shorter than {KebabConstants.MAX_KEBABDESCRIPTION_LENGTH}");
        }

        if (kebabPrice < 0)
        {
            return Result.Failure<Kebab>($"'{nameof(kebabPrice)}' can't be negative");
        }

        if (!Enum.IsDefined(typeof(StuffingCategory), stuffing))
        {
            return Result.Failure<Kebab>($"'{nameof(stuffing)}' is not a valid value");
        }

        if (!Enum.IsDefined(typeof(WrapCategory), wrap))
        {
            return Result.Failure<Kebab>($"'{nameof(wrap)}' is not a valid value");
        }

        if (ingredients == null || ingredients.Count == 0)
        {
            return Result.Failure<Kebab>($"'{nameof(ingredients)}' are required");
        }

        foreach (var ingredient in ingredients)
        {
            if (ingredient == null)
            {
                return Result.Failure<Kebab>("All ingredients must be valid");
            }
        }

        var totalWeight = nutritionAndWeightCalculatorService.CalculateTotalWeight(ingredientWeights);
        var totalCalories = nutritionAndWeightCalculatorService.CalculateTotalCalories(ingredients, ingredientWeights);
        var totalCarbs = nutritionAndWeightCalculatorService.CalculateTotalCarbs(ingredients, ingredientWeights);
        var totalFat = nutritionAndWeightCalculatorService.CalculateTotalFat(ingredients, ingredientWeights);
        var totalProtein = nutritionAndWeightCalculatorService.CalculateTotalProtein(ingredients, ingredientWeights);

        if (totalWeight < 0 || totalCalories < 0 || totalCarbs < 0 || totalFat < 0 || totalProtein < 0)
        {
            return Result.Failure<Kebab>("Nutritional values and total weight can't be negative");
        }

        return Result.Success(new Kebab(id, kebabName, kebabDescription, kebabPrice, stuffing, wrap, isAvailable,
            titleImage, ingredients, totalWeight, totalCalories, totalCarbs, totalFat, totalProtein));
    }
}