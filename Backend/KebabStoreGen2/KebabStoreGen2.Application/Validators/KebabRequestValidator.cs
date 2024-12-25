using FluentValidation;
using KebabStoreGen2.Core.Constants;
using KebabStoreGen2.Core.Contracts;


namespace KebabStoreGen2.Application.Validators;

public class KebabRequestValidator : AbstractValidator<KebabRequest>
{
    public KebabRequestValidator()
    {
        RuleFor(request => request.KebabName)
            .NotEmpty()
            .MaximumLength(KebabConstants.MAX_KEBABNAME_LENGTH)
            .WithMessage("Name is required and must be shorter than 32");

        RuleFor(request => request.KebabDescription)
            .NotEmpty()
            .MaximumLength(KebabConstants.MAX_KEBABDESCRIPTION_LENGTH)
            .WithMessage("Description is required and must be shorter than 100");

        RuleFor(request => request.KebabPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price is required and must be greater or equal to 0");

        RuleFor(request => request.Stuffing)
            .IsInEnum()
            .WithMessage("Stuffing is required and must be a valid value");

        RuleFor(request => request.Wrap)
            .IsInEnum()
            .WithMessage("Wrap is required and must be a valid value");

        RuleFor(request => request.IsAvailable)
            .NotNull()
            .WithMessage("Availability status is required");

        RuleFor(request => request.TitleImage)
            .NotNull()
            .WithMessage("TitleImage is required");

        RuleFor(request => request.TitleImage.ContentType)
            .Must(contentType => contentType == "image/jpeg" || contentType == "image/png")
            .WithMessage("Title image must be a JPEG or PNG");

        RuleForEach(request => request.Ingredients)
            .SetValidator(new KebabIngredientRequestValidator())
            .When(request => request.Ingredients != null);
    }
}