using FluentValidation;
using KebabStoreGen2.Core.Contracts;
using KebabStoreGen2.Core.Models;


namespace KebabStoreGen2.Validation.Validators;

public class KebabsRequestValidator : AbstractValidator<KebabsRequest>
{
    public KebabsRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(Kebab.MAX_KEBABNAME_LENGTH)
            .WithMessage("Name is required and must be shorter than 32");
        RuleFor(request => request.Description)
            .NotEmpty()
            .MaximumLength(Kebab.MAX_KEBABDESCRIPTION_LENGTH)
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
