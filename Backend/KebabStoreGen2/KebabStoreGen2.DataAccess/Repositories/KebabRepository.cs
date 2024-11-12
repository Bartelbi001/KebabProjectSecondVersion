using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Models;
using KebabStoreGen2.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace KebabStoreGen2.DataAccess.Repositories;

public class KebabRepository : IKebabRepository
{
    private readonly KebabStoreGen2DbContext _context;
    private readonly INutritionCalculatorService _calculateTotalCaloriesService;
    private readonly IImageService _imageService;

    public KebabRepository(KebabStoreGen2DbContext context, INutritionCalculatorService calculateTotalCaloriesService, IImageService imageService)
    {
        _context = context;
        _calculateTotalCaloriesService = calculateTotalCaloriesService;
        _imageService = imageService;
    }

    public async Task<Guid> Create(KebabEntity kebabEntity, IFormFile? titleImage = null, string imagePath = "")
    {
        var ingredientEntities = await _context.IngredientEntities
            .Where(i =>
                kebabEntity.Ingredients
                .Select(ing => ing.Id)
                .Contains(i.Id))
            .ToListAsync();

        if (!ingredientEntities.Any())
        {
            throw new KeyNotFoundException("No valid ingredients found");
        }

        var ingredients = ingredientEntities
            .Select(i => Ingredient.Create(
                i.Id,
                i.IngredientName,
                i.WeightInGrams,
                i.Calories,
                i.Protein,
                i.Fat,
                i.Carbs
                ).Value).ToList();

        var calculatedCalories = _calculateTotalCaloriesService.CalculateTotalCalories(ingredients);

        Image? image = null;
        if (titleImage != null)
        {
            var imageResult = await _imageService.CreateImage(titleImage, imagePath);

            if (imageResult.IsFailure)
            {
                throw new Exception(imageResult.Error);
            }

            image = imageResult.Value;
        }

        var kebab = Kebab.Create(
            Guid.NewGuid(),
            kebabEntity.KebabName,
            kebabEntity.KebabDescription,
            kebabEntity.KebabPrice,
            kebabEntity.Stuffing,
            kebabEntity.Wrap,
            kebabEntity.IsAvailable,
            image,
            ingredients,
            _calculateTotalCaloriesService
            );

        if (kebab.IsFailure)
        {
            throw new Exception(kebab.Error);
        }

        kebabEntity.Id = kebab.Value.Id;
        kebabEntity.Calories = calculatedCalories;
        kebabEntity.Ingredients = ingredientEntities;
        kebabEntity.TitleImagePath = image?.Path;

        await _context.KebabEntities.AddAsync(kebabEntity);
        await _context.SaveChangesAsync();

        return kebabEntity.Id;
    }

    public async Task<KebabEntity> GetById(Guid id)
    {
        return await _context.KebabEntities
            .Include(k => k.Ingredients)
            .FirstOrDefaultAsync(k => k.Id == id);
    }

    public async Task<List<KebabEntity>> GetAll()
    {
        return await _context.KebabEntities
            .Include(k => k.Ingredients)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task Update(KebabEntity kebabEntity, string? titleImagePath = null, IFormFile? titleImage = null, string imagePath = "")
    {
        var existingKebab = await _context.KebabEntities
            .Include(k => k.Ingredients)
            .FirstOrDefaultAsync(k => k.Id == kebabEntity.Id);

        if (existingKebab == null)
        {
            throw new KeyNotFoundException("Kebab not found");
        }

        _context.Entry(existingKebab).CurrentValues.SetValues(kebabEntity);

        existingKebab.Ingredients.Clear();
        existingKebab.Ingredients.AddRange(kebabEntity.Ingredients);



        var ingredientEntities = existingKebab.Ingredients
            .Select(i => Ingredient.Create(
                i.Id,
                i.IngredientName,
                i.WeightInGrams,
                i.Calories,
                i.Protein,
                i.Fat,
                i.Carbs).Value).ToList();

        var calculatedCalories = _calculateTotalCaloriesService.CalculateTotalCalories(ingredientEntities);
        existingKebab.Calories = calculatedCalories;

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

        await _context.SaveChangesAsync();
    }

    public async Task<Guid> Delete(Guid id)
    {
        var kebabEntity = await _context.KebabEntities.FindAsync(id);

        if (kebabEntity == null)
        {
            throw new KeyNotFoundException("Kebab not found");
        }

        _context.KebabEntities.Remove(kebabEntity);
        await _context.SaveChangesAsync();

        return kebabEntity.Id;
    }
}