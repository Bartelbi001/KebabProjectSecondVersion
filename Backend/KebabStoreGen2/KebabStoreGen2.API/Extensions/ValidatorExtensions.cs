using FluentValidation;
using KebabStoreGen2.Core.Contracts;
using KebabStoreGen2.Validation.Validators;

namespace KebabStoreGen2.API.Extensions;

public static class ValidatorExtensions
{
    public static void AddValidatorServices(this IServiceCollection services)
    {
        services.AddTransient<IValidator<KebabsRequest>, KebabsRequestValidator>();
    }
}