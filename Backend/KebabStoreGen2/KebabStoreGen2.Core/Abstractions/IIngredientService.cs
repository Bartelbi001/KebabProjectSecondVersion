using KebabStoreGen2.Core.Contracts;
using KebabStoreGen2.Core.Models;

namespace KebabStoreGen2.Core.Abstractions;

public interface IIngredientService
{
    Task<Guid> CreateIngredient(IngredientRequest ingredientRequest);
    Task<IngredientResponse> GetIngredientById(Guid id);
    Task<List<Ingredient>> GetAllIngredients();
    Task UpdateIngredient(Guid id, IngredientRequest ingredientRequest);
    Task<Guid> DeleteIngredient(Guid id);
}