using KebabStoreGen2.Core.Models;

namespace KebabStoreGen2.Core.Abstractions;

public interface IIngredientService
{
    Task<Guid> CreateIngredient(Ingredient ingredient);
    Task<Ingredient> GetIngredientById(Guid id);
    Task<List<Ingredient>> GetAllIngredients();
    Task UpdateIngredient(Guid id, string ingredientName, decimal caloriesPer100g, decimal proteinPer100g, decimal fatPer100g, decimal carbsPer100g, decimal? sugarPer100g, bool? containsLactose);
    Task<Guid> DeleteIngredient(Guid id);
}