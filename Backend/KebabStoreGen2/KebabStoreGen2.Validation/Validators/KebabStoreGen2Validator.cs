using FluentValidation;
using KebabStoreGen2.Core.Contracts;
using KebabStoreGen2.Core.Models;


namespace KebabStoreGen2.Validation.Validators;

public class KebabStoreGen2Validator : AbstractValidator<KebabsRequest>
{
    public KebabStoreGen2Validator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Kebab.MAX_NAME_LENGTH)
            .WithMessage("Name is required and must be shorter than 32");
        RuleFor(request => request.Description)
            .NotEmpty()
            .MaximumLength(Kebab.MAX_DESCRIPTION_LENGTH)
            .WithMessage("Description is required and must be shorter than 100");
        RuleFor(request => request.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price is required and must be greater or equal to 0");
        RuleFor(request => request.TitleImage)
            .NotNull()
            .WithMessage("TitleImage is required");
        RuleFor(request => request.TitleImage.ContentType)
            .Must(contentType => contentType == "image/jpeg" || contentType == "image/png")
            .WithMessage("Title image must be a JPEG or PNG");
    }
}
