using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Contracts;
using KebabStoreGen2.Core.Models;
using KebabStoreGen2.Core.Models.Enums;
using KebabStoreGen2.DataAccess.Entities;

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

    public async Task<Guid> CreateKebab(string kebabName, string kebabDescription, decimal price, StuffingCategory stuffing, WrapCategory wrap,
        bool isAvailable, string? fileName, string? imagePath, List<Guid> existingIngredientIds,
        List<IngredientRequest> newIngredients)
    {
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
            Guid.NewGuid(),
            kebabName,
            kebabDescription,
            price,
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

        var kebabEntity = new KebabEntity
        {
            Id = kebabResult.Value.Id,
            KebabName = kebabResult.Value.KebabName,
            KebabDescription = kebabResult.Value.KebabDescription,
            Price = kebabResult.Value.Price,
            Stuffing = kebabResult.Value.Stuffing,
            Wrap = kebabResult.Value.Wrap,
            IsAvailable = kebabResult.Value.IsAvailable,
            Ingredients = ingredientEntities,
            Calories = kebabResult.Value.Calories,
            TitleImagePath = kebabResult.Value.TitleImage?.Path,
        };

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

    public async Task UpdateKebab(Guid id, string kebabName, string kebabDescription, decimal price, StuffingCategory stuffing,
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
            price,
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
        kebabEntity.Price = kebabResult.Value.Price;
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