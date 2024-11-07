using CSharpFunctionalExtensions;
using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Constants;
using KebabStoreGen2.Core.Models.Enums;

namespace KebabStoreGen2.Core.Models;

public class Kebab
{
    private Kebab(Guid id, string name, string description, decimal price,
        StuffingCategory stuffing, WrapCategory wrap, bool isAvailable,
        Image? titleImage, List<Ingredient> ingredients, int calories)
    {
        Id = id;
        KebabName = name;
        KebabDescription = description;
        Price = price;
        Stuffing = stuffing;
        Wrap = wrap;
        IsAvailable = isAvailable;
        TitleImage = titleImage;
        Ingredients = ingredients;
        Calories = calories;
    }

    public Guid Id { get; }
    public string KebabName { get; private set; } = string.Empty;
    public string KebabDescription { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public StuffingCategory Stuffing { get; private set; }
    public WrapCategory Wrap { get; private set; }
    public bool IsAvailable { get; private set; }
    public Image? TitleImage { get; private set; }
    public List<Ingredient> Ingredients { get; private set; }
    public int Calories { get; private set; }

    public static Result<Kebab> Create(Guid id, string kebabName, string kebabDescription, decimal price,
        StuffingCategory stuffing, WrapCategory wrap, bool isAvailable,
        Image? titleImage, List<Ingredient> ingredients, INutritionCalculatorService calculateTotalCaloriesService)
    {
        if (string.IsNullOrWhiteSpace(kebabName) || kebabName.Length > KebabConstants.MAX_KEBABNAME_LENGTH)
        {
            return Result.Failure<Kebab>($"'{nameof(kebabName)}' is required and must be shorter than {KebabConstants.MAX_KEBABNAME_LENGTH}");
        }

        if (string.IsNullOrWhiteSpace(kebabDescription) || kebabDescription.Length > KebabConstants.MAX_KEBABDESCRIPTION_LENGTH)
        {
            return Result.Failure<Kebab>($"'{nameof(kebabDescription)}' is required and must be shorter than {KebabConstants.MAX_KEBABDESCRIPTION_LENGTH}");
        }

        if (price < 0)
        {
            return Result.Failure<Kebab>($"'{nameof(price)}' can't be negative");
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

        var calculatedCalories = calculateTotalCaloriesService.CalculateTotalCalories(ingredients);
        if (calculatedCalories < 0)
        {
            return Result.Failure<Kebab>($"'{nameof(calculatedCalories)}' can't be negative");
        }

        return Result.Success(new Kebab(id, kebabName, kebabDescription, price,
            stuffing, wrap, isAvailable, titleImage, ingredients, calculatedCalories));
    }
}