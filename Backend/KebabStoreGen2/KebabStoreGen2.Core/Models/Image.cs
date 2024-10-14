using CSharpFunctionalExtensions;

namespace KebabStoreGen2.API.KebabStoreGen2.Core.Models;

public class Image
{
    public Image(string fileName)
    {
        FileName = fileName;
    }

    public Guid KebabId { get; }
    public string FileName { get; } = string.Empty;

    public static Result<Image> Create(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return Result.Failure<Image>($"'{nameof(fileName)}' can't be null or empty");
        }

        var image = new Image(fileName);

        return Result.Success<Image>(image);
    }
}
