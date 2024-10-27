using KebabStoreGen2.Core.Models;

namespace KebabStoreGen2.Core.Abstractions;

public interface IKebabService
{
    Task<Guid> CreateKebab(Kebab kebab);
    Task<Guid> DeleteKebab(Guid id);
    Task<List<Kebab>> GetAllKebabs();
    Task<Kebab> GetKebabById(Guid id);
    Task<Guid> UpdateKebab(Guid id, string kebabName, string kebabDescription, decimal price, string? titleImagePath = null);
}