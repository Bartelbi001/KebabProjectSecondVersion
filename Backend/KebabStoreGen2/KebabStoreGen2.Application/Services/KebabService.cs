using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Models;
using KebabStoreGen2.DataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace KebabStoreGen2.Application.Services;

public class KebabService : IKebabService
{
    private readonly IKebabRepository _kebabRepository;
    private readonly IIngredientRepository _ingredientRepository;
    private readonly INutritionCalculatorService _nutritionCalculatorService;
    private readonly IImageService _imageService;

    public KebabService(IKebabRepository kebabRepository, IIngredientRepository ingredientRepository,
        INutritionCalculatorService nutritionCalculatorService, IImageService imageService)
    {
        _kebabRepository = kebabRepository;
        _ingredientRepository = ingredientRepository;
        _nutritionCalculatorService = nutritionCalculatorService;
        _imageService = imageService;
    }

    public async Task<Guid> CreateKebab(Kebab kebab, IFormFile? titleImage = null, string imagePath = "")
    {
        var ingredients = new List<IngredientEntity>();

        foreach (var ingredient in kebab.Ingredients)
        {
            var ingredientEntity = await _ingredientRepository.GetById(ingredient.Id);
            if (ingredientEntity == null)
            {
                ingredientEntity = new IngredientEntity
                {
                    Id = ingredient.Id,
                    IngredientName = ingredient.IngredientName,
                    WeightInGrams = ingredient.WeightInGrams,
                    Calories = ingredient.Calories,
                    Protein = ingredient.Protein,
                    Fat = ingredient.Fat,
                    Carbs = ingredient.Carbs,
                };
                await _ingredientRepository.Create(ingredientEntity);
            }
            ingredients.Add(ingredientEntity);
        }

        var ingredientModels = ingredients
            .Select(ie => Ingredient.Create(
                ie.Id,
                ie.IngredientName,
                ie.WeightInGrams,
                ie.Calories,
                ie.Protein,
                ie.Fat,
                ie.Carbs).Value).ToList();

        var kebabEntity = new KebabEntity
        {
            Id = kebab.Id,
            KebabName = kebab.KebabName,
            KebabDescription = kebab.KebabDescription,
            Price = kebab.Price,
            Stuffing = kebab.Stuffing,
            Wrap = kebab.Wrap,
            IsAvailable = kebab.IsAvailable,
            Ingredients = ingredients,
            Calories = _nutritionCalculatorService.CalculateTotalCalories(ingredientModels)
        };

        if (titleImage != null)
        {
            var imageResult = await _imageService.CreateImage(titleImage, imagePath);
            if (imageResult.IsFailure)
            {
                throw new Exception(imageResult.Error);
            }
            kebabEntity.TitleImagePath = imageResult.Value.Path;
        }

        return await _kebabRepository.Create(kebabEntity);
    }

    public async Task<Kebab> GetKebabById(Guid id)
    {
        var kebabEntity = await _kebabRepository.GetById(id);
        if (kebabEntity == null)
        {
            throw new KeyNotFoundException("Kebab not found");
        }

        var ingredients = kebabEntity.Ingredients
            .Select(ie => Ingredient.Create(
                ie.Id,
                ie.IngredientName,
                ie.WeightInGrams,
                ie.Calories,
                ie.Protein,
                ie.Fat,
                ie.Carbs).Value)
            .ToList();

        var image = !string.IsNullOrEmpty(kebabEntity.TitleImagePath)
            ? Image.Create(Path.GetFileName(kebabEntity.TitleImagePath), kebabEntity.TitleImagePath).Value
            : null;

        return Kebab.Create(
            kebabEntity.Id,
            kebabEntity.KebabName,
            kebabEntity.KebabDescription,
            kebabEntity.Price,
            kebabEntity.Stuffing,
            kebabEntity.Wrap,
            kebabEntity.IsAvailable,
            image,
            ingredients,
            _nutritionCalculatorService).Value;
    }

    public async Task<List<Kebab>> GetAllKebabs()
    {
        var kebabEntities = await _kebabRepository.GetAll();

        return kebabEntities.Select(kebabEntity =>
        {
            var ingredients = kebabEntity.Ingredients
                .Select(ie => Ingredient.Create(
                    ie.Id,
                    ie.IngredientName,
                    ie.WeightInGrams,
                    ie.Calories,
                    ie.Protein,
                    ie.Fat,
                    ie.Carbs).Value)
                .ToList();

            var image = !string.IsNullOrEmpty(kebabEntity.TitleImagePath)
            ? Image.Create(Path.GetFileName(kebabEntity.TitleImagePath), kebabEntity.TitleImagePath).Value
            : null;

            return Kebab.Create(
                kebabEntity.Id,
                kebabEntity.KebabName,
                kebabEntity.KebabDescription,
                kebabEntity.Price,
                kebabEntity.Stuffing,
                kebabEntity.Wrap,
                kebabEntity.IsAvailable,
                image,
                ingredients,
                _nutritionCalculatorService).Value;
        }).ToList();
    }

    public async Task UpdateKebab(Kebab kebab, string? titleImagePath = null, IFormFile? titleImage = null, string imagePath = "")
    {
        var existingKebab = await _kebabRepository.GetById(kebab.Id);
        if (existingKebab == null)
        {
            throw new KeyNotFoundException("Kebab not found");
        }

        var ingredients = new List<IngredientEntity>();
        foreach (var ingredient in kebab.Ingredients)
        {
            var ingredientEntity = await _ingredientRepository.GetById(ingredient.Id);
            if (ingredientEntity == null)
            {
                ingredientEntity = new IngredientEntity
                {
                    Id = ingredient.Id,
                    IngredientName = ingredient.IngredientName,
                    WeightInGrams = ingredient.WeightInGrams,
                    Calories = ingredient.Calories,
                    Protein = ingredient.Protein,
                    Fat = ingredient.Fat,
                    Carbs = ingredient.Carbs,
                };
                await _ingredientRepository.Create(ingredientEntity);
            }
            ingredients.Add(ingredientEntity);
        }

        var ingredientModels = ingredients
            .Select(ie => Ingredient.Create(
                ie.Id,
                ie.IngredientName,
                ie.WeightInGrams,
                ie.Calories,
                ie.Protein,
                ie.Fat,
                ie.Carbs).Value)
            .ToList();

        existingKebab.KebabName = kebab.KebabName;
        existingKebab.KebabDescription = kebab.KebabDescription;
        existingKebab.Price = kebab.Price;
        existingKebab.Stuffing = kebab.Stuffing;
        existingKebab.Wrap = kebab.Wrap;
        existingKebab.IsAvailable = kebab.IsAvailable;
        existingKebab.Ingredients = ingredients;
        existingKebab.Calories = _nutritionCalculatorService.CalculateTotalCalories(ingredientModels);

        if (titleImage != null)
        {
            var imageResult = await _imageService.CreateImage(titleImage, imagePath);
            if (imageResult.IsFailure)
            {
                throw new Exception(imageResult.Error);
            }
            existingKebab.TitleImagePath = imageResult.Value.Path;
        }
        else if (!string.IsNullOrWhiteSpace(titleImagePath))
        {
            existingKebab.TitleImagePath = titleImagePath;
        }

        await _kebabRepository.Update(existingKebab);
    }

    public async Task<Guid> DeleteKebab(Guid id)
    {
        var kebabEntity = await _kebabRepository.GetById(id);
        if (kebabEntity == null)
        {
            throw new KeyNotFoundException("Kebab not found");
        }

        return await _kebabRepository.Delete(id);
    }
}