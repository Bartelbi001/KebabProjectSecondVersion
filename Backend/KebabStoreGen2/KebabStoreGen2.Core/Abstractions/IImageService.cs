using CSharpFunctionalExtensions;
using KebabStoreGen2.Core.Models;
using Microsoft.AspNetCore.Http;

namespace KebabStoreGen2.Core.Abstractions;

public interface IImageService
{
    Task<Result<Image>> CreateImage(IFormFile titleImage, string path);
}
