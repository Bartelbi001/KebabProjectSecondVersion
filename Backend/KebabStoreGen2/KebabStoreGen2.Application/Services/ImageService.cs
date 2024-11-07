using CSharpFunctionalExtensions;
using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Models;
using Microsoft.AspNetCore.Http;

namespace KebabStoreGen2.Application.Services;

public class ImageService : IImageService
{
    public async Task<Result<Image>> CreateImage(IFormFile titleImage, string path)
    {
        try
        {
            var fileName = Path.GetFileName(titleImage.FileName);
            var filePath = Path.Combine(path, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await titleImage.CopyToAsync(stream);
            }

            var image = Image.Create(fileName, filePath);

            return image;
        }
        catch (Exception ex)
        {
            return Result.Failure<Image>(ex.Message);
        }
    }
}