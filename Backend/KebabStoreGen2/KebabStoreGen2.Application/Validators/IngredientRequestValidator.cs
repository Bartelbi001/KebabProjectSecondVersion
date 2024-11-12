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

        RuleFor(request => request.WeightInGrams)
            .GreaterThan(0)
            .WithMessage("Weight must be greater than 0");

        RuleFor(request => request.Calories)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Calories must be greater than 0");

        RuleFor(request => request.Protein)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Protein must be greater than 0");

        RuleFor(request => request.Fat)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Fat must be greater than 0");

        RuleFor(request => request.Carbs)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Carbs must be greater than 0");
    }
}