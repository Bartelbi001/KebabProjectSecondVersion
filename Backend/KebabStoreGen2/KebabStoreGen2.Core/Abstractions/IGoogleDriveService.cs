using CSharpFunctionalExtensions;
using KebabStoreGen2.Core.Models;
using Microsoft.AspNetCore.Http;

namespace KebabStoreGen2.Core.Abstractions;

public interface IGoogleDriveService
{
    Task<Result<Image>> CreateImage(IFormFile file, string fileName);
}
