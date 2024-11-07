using KebabStoreGen2.Core.Models;

namespace KebabStoreGen2.Core.Abstractions;

public interface IIngredientService
{
    Task<Guid> CreateIngredient(Ingredient ingredient);
    Task<Ingredient> GetIngredientById(Guid id);
    Task<List<Ingredient>> GetAllIngredients();
    Task UpdateIngredient(Guid id, string ingredientName, int weightInGrams, int calories, int protein, int fat, int carbs);
    Task<Guid> DeleteIngredient(Guid id);
}