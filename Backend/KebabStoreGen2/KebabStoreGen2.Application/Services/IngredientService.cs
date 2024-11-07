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
            WeightInGrams = ingredient.WeightInGrams,
            Calories = ingredient.Calories,
            Protein = ingredient.Protein,
            Fat = ingredient.Fat,
            Carbs = ingredient.Carbs,
        };

        return await _ingredientRepository.Create(ingredientEntity);
    }

    public async Task<Ingredient> GetIngredientById(Guid id)
    {
        var ingredientEntity = await _ingredientRepository.GetById(id);
        if (ingredientEntity == null)
        {
            throw new KeyNotFoundException("Ingredient not found");
        }

        return Ingredient.Create(
            ingredientEntity.Id,
            ingredientEntity.IngredientName,
            ingredientEntity.WeightInGrams,
            ingredientEntity.Calories,
            ingredientEntity.Protein,
            ingredientEntity.Fat,
            ingredientEntity.Carbs).Value;
    }

    public async Task<List<Ingredient>> GetAllIngredients()
    {
        var ingredientEntities = await _ingredientRepository.GetAll();
        return ingredientEntities.Select(ie => Ingredient.Create(
            ie.Id,
            ie.IngredientName,
            ie.WeightInGrams,
            ie.Calories,
            ie.Protein,
            ie.Fat,
            ie.Carbs).Value).ToList();
    }

    public async Task UpdateIngredient(Guid id, string ingredientName, int weightInGrams, int calories,
        int protein, int fat, int carbs)
    {
        var ingredientEntity = await _ingredientRepository.GetById(id);
        if (ingredientEntity == null)
        {
            throw new KeyNotFoundException("Ingredient not found");
        }

        ingredientEntity.IngredientName = ingredientName;
        ingredientEntity.WeightInGrams = weightInGrams;
        ingredientEntity.Calories = calories;
        ingredientEntity.Protein = protein;
        ingredientEntity.Fat = fat;
        ingredientEntity.Carbs = carbs;

        await _ingredientRepository.Update(ingredientEntity);
    }

    public async Task<Guid> DeleteIngredient(Guid id)
    {
        var ingredientEntity = await _ingredientRepository.GetById(id);
        if (ingredientEntity == null)
        {
            throw new KeyNotFoundException("Ingredient not found");
        }

        await _ingredientRepository.Delete(id);
        return id;
    }
}