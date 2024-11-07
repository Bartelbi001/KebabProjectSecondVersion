using KebabStoreGen2.Core.Models;
using Microsoft.AspNetCore.Http;

namespace KebabStoreGen2.Core.Abstractions;

public interface IKebabService
{
    Task<Guid> CreateKebab(Kebab kebab, IFormFile? titleImage = null, string imagePath = "");
    Task<Kebab> GetKebabById(Guid id);
    Task<List<Kebab>> GetAllKebabs();
    Task UpdateKebab(Kebab kebab, string? titleImagePath = null, IFormFile? titleImage = null, string imagePath = "");
    Task<Guid> DeleteKebab(Guid id);
}