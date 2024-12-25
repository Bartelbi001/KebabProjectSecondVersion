using FluentValidation;
using KebabStoreGen2.Core.Constants;
using KebabStoreGen2.Core.Contracts;

namespace KebabStoreGen2.Application.Validators;

public class KebabIngredientRequestValidator : AbstractValidator<KebabIngredientRequest>
{
    public KebabIngredientRequestValidator()
    {
        RuleFor(request => request.IngredientName)
             .NotEmpty()
            .MaximumLength(IngredientConstants.MAX_INGREDIENTNAME_LENGTH)
            .WithMessage("Name is required and must be shorter than 16");

        RuleFor(request => request.Weight)
            .GreaterThan(0)
            .WithMessage("Weight must be greater than 0");

        RuleFor(request => request.CaloriesPer100g)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Calories per 100g must be greater than or equal to 0");

        RuleFor(request => request.ProteinPer100g)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Protein per 100g must be greater than or equal to 0");

        RuleFor(request => request.FatPer100g)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Fat per 100g must be greater than or equal to 0");

        RuleFor(request => request.CarbsPer100g)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Carbs per 100g must be greater than or equal to 0");

        RuleFor(request => request.SugarPer100g)
            .GreaterThanOrEqualTo(0)
            .When(request => request.SugarPer100g.HasValue)
            .WithMessage("Sugar per 100g must be greater than or equal to 0");

        RuleFor(request => request.ContainsLactose)
            .NotNull()
            .When(request => request.ContainsLactose.HasValue)
            .WithMessage("ContainsLactose must be specified if present");
    }
}