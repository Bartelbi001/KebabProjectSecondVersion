using CSharpFunctionalExtensions;

namespace KebabStoreGen2.Core.Models;

public class Image
{
    private Image(string fileName, string path)
    {
        FileName = fileName;
        Path = path;
    }

    public Guid KebabId { get; }
    public string FileName { get; } = string.Empty;
    public string Path { get; } = string.Empty; // Добавляем свойство Path

    public static Result<Image> Create(string fileName, string path)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return Result.Failure<Image>($"'{nameof(fileName)}' can't be null or empty");
        }

        if (string.IsNullOrEmpty(path))
        {
            return Result.Failure<Image>($"'{nameof(path)}' can't be null or empty");
        }

        return Result.Success(new Image(fileName, path));
    }
}
