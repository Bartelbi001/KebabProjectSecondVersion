using FluentValidation;
using KebabStoreGen2.Core.Contracts;

namespace KebabStoreGen2.Application.Validators;

public class IngredientRequestValidator : AbstractValidator<IngredientRequest>
{
    public IngredientRequestValidator()
    {
        RuleFor(request => request.IngredientName)
            .NotEmpty()
            .WithMessage("Ingredient name is required");

        RuleFor(request => request.CaloriesPer100g)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Calories per 100g must be greater than or equal to 0");

        RuleFor(request => request.ProteinPer100g)
            .GreaterThanOrEqualTo(0)
            .WithMessage("\"Protein per 100g must be greater than or equal to 0");

        RuleFor(request => request.FatPer100g)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Fat per 100g must be greater than or equal to 0");

        RuleFor(request => request.CarbsPer100g)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Carbs per 100g must be greater than or equal to 0");
    }
}