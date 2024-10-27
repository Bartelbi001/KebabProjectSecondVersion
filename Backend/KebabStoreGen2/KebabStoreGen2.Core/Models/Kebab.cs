using CSharpFunctionalExtensions;

namespace KebabStoreGen2.Core.Models;

public class Kebab
{
    public const int MAX_KEBABNAME_LENGTH = 32;
    public const int MAX_KEBABDESCRIPTION_LENGTH = 100;

    private Kebab(Guid id, string name, string description, decimal price, Image? titleImage)
    {
        Id = id;
        KebabName = name;
        KebabDescription = description;
        Price = price;
        TitleImage = titleImage;
    }

    public Guid Id { get; }
    public string KebabName { get; } = string.Empty;
    public string KebabDescription { get; } = string.Empty;
    public decimal Price { get; }
    public Image? TitleImage { get; }

    public static Result<Kebab> Create(Guid id, string kebabName, string kebabDescription, decimal price, Image? titleImage)
    {
        if (string.IsNullOrWhiteSpace(kebabName) || kebabName.Length > MAX_KEBABNAME_LENGTH)
        {
            return Result.Failure<Kebab>($"'{nameof(kebabName)}' is required and must be shorter than {MAX_KEBABNAME_LENGTH}");
        }

        if (string.IsNullOrWhiteSpace(kebabDescription) || kebabDescription.Length > MAX_KEBABDESCRIPTION_LENGTH)
        {
            return Result.Failure<Kebab>($"'{nameof(kebabDescription)}' is required and must be shorter than {MAX_KEBABDESCRIPTION_LENGTH}");
        }

        if (price < 0)
        {
            return Result.Failure<Kebab>($"'{nameof(price)}' can't be negative");
        }

        return Result.Success(new Kebab(id, kebabName, kebabDescription, price, titleImage));
    }
}