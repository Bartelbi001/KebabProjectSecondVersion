using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Contracts;
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

    public async Task<Guid> CreateIngredient(IngredientRequest ingredientRequest)
    {
        var ingredientResult = Ingredient.Create(
            Guid.NewGuid(),
            ingredientRequest.IngredientName,
            ingredientRequest.CaloriesPer100g,
            ingredientRequest.ProteinPer100g,
            ingredientRequest.FatPer100g,
            ingredientRequest.CarbsPer100g,
            ingredientRequest.SugarPer100g ?? 0,
            ingredientRequest.ContainsLactose ?? false
        );

        if (ingredientResult.IsFailure)
        {
            throw new InvalidOperationException(ingredientResult.Error);
        }

        var ingredientEntity = new IngredientEntity
        {
            Id = ingredientResult.Value.Id,
            IngredientName = ingredientResult.Value.IngredientName,
            CaloriesPer100g = ingredientResult.Value.CaloriesPer100g,
            ProteinPer100g = ingredientResult.Value.ProteinPer100g,
            FatPer100g = ingredientResult.Value.FatPer100g,
            CarbsPer100g = ingredientResult.Value.CarbsPer100g,
            SugarPer100g = ingredientResult.Value.SugarPer100g,
            ContainsLactose = ingredientResult.Value.ContainsLactose
        };

        await _ingredientRepository.Create(ingredientEntity);
        return ingredientEntity.Id;
    }

    public async Task<IngredientResponse> GetIngredientById(Guid id)
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

        return new IngredientResponse
        {
            Id = ingredientResult.Value.Id,
            IngredientName = ingredientResult.Value.IngredientName,
            CaloriesPer100g = ingredientResult.Value.CaloriesPer100g,
            ProteinPer100g = ingredientResult.Value.ProteinPer100g,
            FatPer100g = ingredientResult.Value.FatPer100g,
            CarbsPer100g = ingredientResult.Value.CarbsPer100g,
            SugarPer100g = ingredientResult.Value.SugarPer100g,
            ContainsLactose = ingredientResult.Value.ContainsLactose,
        };
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

    public async Task UpdateIngredient(Guid id, IngredientRequest ingredientRequest)
    {
        var ingredientEntity = await _ingredientRepository.GetById(id);
        if (ingredientEntity == null)
        {
            throw new KeyNotFoundException($"Ingredient with id {id} not found");
        }

        var ingredientResult = Ingredient.Create(
            id,
            ingredientRequest.IngredientName,
            ingredientRequest.CaloriesPer100g,
            ingredientRequest.ProteinPer100g,
            ingredientRequest.FatPer100g,
            ingredientRequest.CarbsPer100g,
            ingredientRequest.SugarPer100g ?? 0,
            ingredientRequest.ContainsLactose ?? false
        );

        if (ingredientResult.IsFailure)
        {
            throw new InvalidOperationException(ingredientResult.Error);
        }

        var ingredient = ingredientResult.Value;

        ingredientEntity.IngredientName = ingredient.IngredientName;
        ingredientEntity.CaloriesPer100g = ingredient.CaloriesPer100g;
        ingredientEntity.ProteinPer100g = ingredient.ProteinPer100g;
        ingredientEntity.FatPer100g = ingredient.FatPer100g;
        ingredientEntity.CarbsPer100g = ingredient.CarbsPer100g;
        ingredientEntity.SugarPer100g = ingredient.SugarPer100g ?? 0;
        ingredientEntity.ContainsLactose = ingredient.ContainsLactose ?? false;

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