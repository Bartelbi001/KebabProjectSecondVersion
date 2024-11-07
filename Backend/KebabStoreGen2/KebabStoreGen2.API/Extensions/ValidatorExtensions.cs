using FluentValidation;
using KebabStoreGen2.Application.Validators;
using KebabStoreGen2.Core.Contracts;

namespace KebabStoreGen2.API.Extensions;

public static class ValidatorExtensions
{
    public static void AddValidatorServices(this IServiceCollection services)
    {
        services.AddTransient<IValidator<KebabRequest>, KebabRequestValidator>();
        services.AddTransient<IValidator<IngredientRequest>, IngredientRequestValidator>();
    }
}