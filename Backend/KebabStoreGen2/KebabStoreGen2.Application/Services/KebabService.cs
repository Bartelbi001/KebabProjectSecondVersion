using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Contracts;
using KebabStoreGen2.Core.Models;
using KebabStoreGen2.Core.Models.Enums;
using KebabStoreGen2.DataAccess.Entities;
using System.Data;

namespace KebabStoreGen2.Application.Services;

public class KebabService : IKebabService
{
    private readonly IKebabRepository _kebabRepository;
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IGoogleDriveService _googleDriveService;
    private readonly INutritionAndWeightCalculatorService _nutritionAndWeightCalculatorService;
    private readonly IImageService _imageService;

    public KebabService(IKebabRepository kebabRepository, IIngredientRepository ingredientRepository, IGoogleDriveService googleDriveService,
        INutritionAndWeightCalculatorService nutritionAndWeightCalculatorService, IImageService imageService)
    {
        _kebabRepository = kebabRepository;
        _ingredientRepository = ingredientRepository;
        _googleDriveService = googleDriveService;
        _nutritionAndWeightCalculatorService = nutritionAndWeightCalculatorService;
        _imageService = imageService;
    }

    public async Task<Guid> CreateKebab(KebabRequest kebabRequest)
    {
        var imageResult = await _googleDriveService.CreateImage(kebabRequest.TitleImage, kebabRequest.KebabName);
        if (imageResult.IsFailure)
        {
            throw new InvalidOperationException(imageResult.Error);
        }

        var ingredients = new List<Ingredient>();
        var weights = new List<int>();

        foreach (var kebabIngredientRequest in kebabRequest.Ingredients)
        {
            Ingredient ingredientEntity;
            if (kebabIngredientRequest.Id.HasValue)
            {
                var ingredientEntityFromDb = await _ingredientRepository.GetById(kebabIngredientRequest.Id.Value);
                if (ingredientEntityFromDb == null)
                {
                    throw new KeyNotFoundException($"Ingredient with id {kebabIngredientRequest.Id} not found");
                }

                ingredientEntity = Ingredient.Create(
                    ingredientEntityFromDb.Id,
                    ingredientEntityFromDb.IngredientName,
                    ingredientEntityFromDb.CaloriesPer100g,
                    ingredientEntityFromDb.ProteinPer100g,
                    ingredientEntityFromDb.FatPer100g,
                    ingredientEntityFromDb.CarbsPer100g,
                    ingredientEntityFromDb.SugarPer100g ?? 0,
                    ingredientEntityFromDb.ContainsLactose ?? false
                ).Value;
            }
            else
            {
                var ingredientResult = Ingredient.Create(
                    Guid.NewGuid(),
                    kebabIngredientRequest.IngredientName,
                    kebabIngredientRequest.CaloriesPer100g,
                    kebabIngredientRequest.ProteinPer100g,
                    kebabIngredientRequest.FatPer100g,
                    kebabIngredientRequest.CarbsPer100g,
                    kebabIngredientRequest.SugarPer100g ?? 0,
                    kebabIngredientRequest.ContainsLactose ?? false
                );

                if (ingredientResult.IsFailure)
                {
                    throw new InvalidOperationException(ingredientResult.Error);
                }

                ingredientEntity = ingredientResult.Value;
                await _ingredientRepository.Create(new IngredientEntity
                {
                    Id = ingredientEntity.Id,
                    IngredientName = ingredientEntity.IngredientName,
                    CaloriesPer100g = ingredientEntity.CaloriesPer100g,
                    ProteinPer100g = ingredientEntity.ProteinPer100g,
                    FatPer100g = ingredientEntity.FatPer100g,
                    CarbsPer100g = ingredientEntity.CarbsPer100g,
                    SugarPer100g = ingredientEntity.SugarPer100g,
                    ContainsLactose = ingredientEntity.ContainsLactose
                });
            }

            ingredients.Add(ingredientEntity);
            weights.Add(kebabIngredientRequest.Weight);
        }

        var image = Image.Create(kebabRequest.TitleImage.FileName, imageResult.Value.Path).Value;

        var kebabResult = Kebab.Create(
            Guid.NewGuid(),
            kebabRequest.KebabName,
            kebabRequest.KebabDescription,
            kebabRequest.KebabPrice,
            kebabRequest.Stuffing,
            kebabRequest.Wrap,
            kebabRequest.IsAvailable,
            image,
            ingredients,
            _nutritionAndWeightCalculatorService,
            weights
        );

        if (kebabResult.IsFailure)
        {
            throw new InvalidOperationException(kebabResult.Error);
        }

        var kebabEntity = kebabResult.Value;

        await _kebabRepository.Create(kebabEntity);
        return kebabEntity.Id;
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
            kebabEntity.KebabPrice,
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
                kebabEntity.KebabPrice,
                kebabEntity.Stuffing,
                kebabEntity.Wrap,
                kebabEntity.IsAvailable,
                image,
                ingredients,
                _nutritionCalculatorService).Value;
        }).ToList();
    }

    public async Task UpdateKebab(Guid id, string kebabName, string kebabDescription, decimal kebabPrice, StuffingCategory stuffing,
        WrapCategory wrap, bool isAvailable, string? fileName, string? imagePath,
        List<Guid> existingIngredientIds, List<IngredientRequest> newIngredients)
    {
        var kebabEntity = await _kebabRepository.GetById(id);
        if (kebabEntity == null)
        {
            throw new KeyNotFoundException("Kebab not found");
        }

        var ingredientEntities = new List<IngredientEntity>();
        var ingredients = new List<Ingredient>();

        if (existingIngredientIds != null && existingIngredientIds.Any())
        {
            var existingIngredients = await _ingredientRepository.GetAll();
            foreach (var entity in existingIngredients.Where(i => existingIngredientIds.Contains(i.Id)))
            {
                var ingredientResult = Ingredient.Create(
                    entity.Id,
                    entity.IngredientName,
                    entity.WeightInGrams,
                    entity.Calories,
                    entity.Protein,
                    entity.Fat,
                    entity.Carbs);

                if (ingredientResult.IsSuccess)
                {
                    ingredients.Add(ingredientResult.Value);
                    ingredientEntities.Add(entity);
                }
            }
        }

        if (newIngredients != null && newIngredients.Any())
        {
            foreach (var ingredientRequest in newIngredients)
            {
                var ingredientResult = Ingredient.Create(
                    Guid.NewGuid(),
                    ingredientRequest.IngredientName,
                    ingredientRequest.WeightInGrams,
                    ingredientRequest.Calories,
                    ingredientRequest.Protein,
                    ingredientRequest.Fat,
                    ingredientRequest.Carbs);

                if (ingredientResult.IsSuccess)
                {
                    var ingredientEntity = new IngredientEntity
                    {
                        Id = ingredientResult.Value.Id,
                        IngredientName = ingredientResult.Value.IngredientName,
                        WeightInGrams = ingredientResult.Value.WeightInGrams,
                        Calories = ingredientResult.Value.Calories,
                        Protein = ingredientResult.Value.Protein,
                        Fat = ingredientResult.Value.Fat,
                        Carbs = ingredientResult.Value.Carbs,
                    };

                    await _ingredientRepository.Create(ingredientEntity);
                    ingredientEntities.Add(ingredientEntity);
                    ingredients.Add(ingredientResult.Value);
                }
            }
        }

        Image? image = null;
        if (image != null && fileName != null)
        {
            var imageResult = Image.Create(fileName, imagePath);
            if (imageResult.IsSuccess)
            {
                image = imageResult.Value;
            }
            else
            {
                throw new InvalidOperationException(imageResult.Error);
            }
        }

        var kebabResult = Kebab.Create(
            kebabEntity.Id,
            kebabName,
            kebabDescription,
            kebabPrice,
            stuffing,
            wrap,
            isAvailable,
            image,
            ingredients,
            _nutritionCalculatorService);
        if (kebabResult.IsFailure)
        {
            throw new InvalidOperationException(kebabResult.Error);
        }

        kebabEntity.KebabName = kebabResult.Value.KebabName;
        kebabEntity.KebabDescription = kebabResult.Value.KebabDescription;
        kebabEntity.KebabPrice = kebabResult.Value.KebabPrice;
        kebabEntity.Stuffing = kebabResult.Value.Stuffing;
        kebabEntity.Wrap = kebabResult.Value.Wrap;
        kebabEntity.IsAvailable = kebabResult.Value.IsAvailable;
        kebabEntity.Ingredients = ingredientEntities;
        kebabEntity.Calories = kebabResult.Value.Calories;
        kebabEntity.TitleImagePath = kebabResult.Value.TitleImage?.Path;

        await _kebabRepository.Update(kebabEntity);
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