using CSharpFunctionalExtensions;

namespace KebabStoreGen2.Core.Models;

public class Kebab
{
    public const int MAX_NAME_LENGTH = 32;
    public const int MAX_DESCRIPTION_LENGTH = 100;

    private Kebab(Guid id, string name, string description, decimal price, Image? titleImage)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        TitleImage = titleImage;
    }

    public Guid Id { get; }
    public string Name { get; } = string.Empty;
    public string Description { get; } = string.Empty;
    public decimal Price { get; }
    public Image? TitleImage { get; }

    public static Result<Kebab> Create(Guid id, string name, string description, decimal price, Image? titleImage)
    {
        if (string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGTH)
        {
            return Result.Failure<Kebab>($"'{nameof(name)}' can't be null or empty");
        }

        if (string.IsNullOrEmpty(description) || description.Length > MAX_DESCRIPTION_LENGTH)
        {
            return Result.Failure<Kebab>($"'{nameof(description)}' can't be null or empty");
        }

        if (price < 0)
        {
            return Result.Failure<Kebab>($"'{nameof(price)}' can't be negative");
        }

        var kebab = new Kebab(id, name, description, price, titleImage);

        return Result.Success(kebab);
    }
}