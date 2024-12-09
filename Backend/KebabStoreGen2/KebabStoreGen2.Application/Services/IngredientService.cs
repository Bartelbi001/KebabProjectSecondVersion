using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Models;
using KebabStoreGen2.DataAccess.Entities;

namespace KebabStoreGen2.Application.Services;

public class IngredientService : IIngredientService
{
    private readonly IIngredientRepository _ingredientRepository;

    public IngredientService(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }

    public async Task<Guid> CreateIngredient(Ingredient ingredient)
    {
        var ingredientEntity = new IngredientEntity
        {
            Id = ingredient.Id,
            IngredientName = ingredient.IngredientName,
            CaloriesPer100g = ingredient.CaloriesPer100g,
            ProteinPer100g = ingredient.ProteinPer100g,
            FatPer100g = ingredient.FatPer100g,
            CarbsPer100g = ingredient.CarbsPer100g,
            SugarPer100g = ingredient.SugarPer100g,
            ContainsLactose = ingredient.ContainsLactose
        };

        await _ingredientRepository.Create(ingredientEntity);
        return ingredient.Id;
    }

    public async Task<Ingredient> GetIngredientById(Guid id)
    {
        var ingredientEntity = await _ingredientRepository.GetById(id);
        if (ingredientEntity == null)
        {
            throw new KeyNotFoundException($"Ingredient with id {id} not found");
        }

        var ingredientResult = Ingredient.Create(
            ingredientEntity.Id,
            ingredientEntity.IngredientName,
            ingredientEntity.CaloriesPer100g,
            ingredientEntity.ProteinPer100g,
            ingredientEntity.FatPer100g,
            ingredientEntity.CarbsPer100g,
            ingredientEntity.SugarPer100g ?? 0,
            ingredientEntity.ContainsLactose ?? false
            );

        if (ingredientResult.IsFailure)
        {
            throw new InvalidOperationException(ingredientResult.Error);
        }

        return ingredientResult.Value;
    }

    public async Task<List<Ingredient>> GetAllIngredients()
    {
        var ingredientEntities = await _ingredientRepository.GetAll();
        var ingredients = ingredientEntities.Select(entity =>
        {
            var result = Ingredient.Create(
                entity.Id,
                entity.IngredientName,
                entity.CaloriesPer100g,
                entity.ProteinPer100g,
                entity.FatPer100g,
                entity.CarbsPer100g,
                entity.SugarPer100g ?? 0,
                entity.ContainsLactose ?? false
            );

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            return result.Value;
        }).ToList();

        return ingredients;
    }

    public async Task UpdateIngredient(Guid id, string ingredientName, decimal caloriesPer100g, decimal proteinPer100g,
        decimal fatPer100g, decimal carbsPer100g, decimal? sugarPer100g, bool? containsLactose)
    {
        var ingredientEntity = await _ingredientRepository.GetById(id);
        if (ingredientEntity == null)
        {
            throw new KeyNotFoundException($"Ingredient with id {id} not found");
        }

        ingredientEntity.IngredientName = ingredientName;
        ingredientEntity.CaloriesPer100g = caloriesPer100g;
        ingredientEntity.ProteinPer100g = proteinPer100g;
        ingredientEntity.FatPer100g = fatPer100g;
        ingredientEntity.CarbsPer100g = carbsPer100g;
        ingredientEntity.SugarPer100g = sugarPer100g ?? 0;
        ingredientEntity.ContainsLactose = containsLactose ?? false;

        await _ingredientRepository.Update(ingredientEntity);
    }

    public async Task<Guid> DeleteIngredient(Guid id)
    {
        var ingredientEntity = await _ingredientRepository.GetById(id);
        if (ingredientEntity == null)
        {
            throw new KeyNotFoundException($"Ingredient with id {id} not found");
        }

        await _ingredientRepository.Delete(id);
        return ingredientEntity.Id;
    }
}