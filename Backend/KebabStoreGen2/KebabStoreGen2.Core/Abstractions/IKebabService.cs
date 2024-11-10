using KebabStoreGen2.Core.Contracts;
using KebabStoreGen2.Core.Models;
using KebabStoreGen2.Core.Models.Enums;

namespace KebabStoreGen2.Core.Abstractions;

public interface IKebabService
{
    Task<Guid> CreateKebab(string kebabName, string kebabDescription, decimal price, StuffingCategory stuffing, WrapCategory wrap,
        bool isAvailable, string? fileName, string? imagePath, List<Guid> existingIngredientIds,
        List<IngredientRequest> newIngredients);
    Task<Kebab> GetKebabById(Guid id);
    Task<List<Kebab>> GetAllKebabs();
    Task UpdateKebab(Guid id, string kebabName, string kebabDescription, decimal price, StuffingCategory stuffing,
        WrapCategory wrap, bool isAvailable, string? fileName, string? imagePath,
        List<Guid> existingIngredientIds, List<IngredientRequest> newIngredients);
    Task<Guid> DeleteKebab(Guid id);
}