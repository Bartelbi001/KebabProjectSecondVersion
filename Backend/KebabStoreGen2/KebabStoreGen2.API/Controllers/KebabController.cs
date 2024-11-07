using CSharpFunctionalExtensions;
using FluentValidation;
using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Contracts;
using KebabStoreGen2.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;

namespace KebabStoreGen2.API.Controllers;

[ApiController]
[Route("[controller]")]
public class KebabController : ControllerBase
{
    private readonly string _staticFilesPath =
        Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles/Images");

    private readonly IKebabService _kebabService;
    private readonly IImageService _imageService;
    private readonly IValidator<KebabRequest> _validator;
    private readonly INutritionCalculatorService _nutritionCalculatorService;

    public KebabController(IKebabService kebabService, IImageService imageService, IValidator<KebabRequest> validator, INutritionCalculatorService nutritionCalculatorService)
    {
        _kebabService = kebabService;
        _imageService = imageService;
        _validator = validator;
        _nutritionCalculatorService = nutritionCalculatorService;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateKebab([FromForm] KebabRequest request)
    {
        var watch = Stopwatch.StartNew();
        Log.Information("Creating a new kebab with Name: {Name}", request.KebabName);

        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            Log.Warning("Validation failed: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        try
        {
            Image? image = null;
            if (request.TitleImage != null)
            {
                var imageResult = await _imageService.CreateImage(request.TitleImage, _staticFilesPath);
                if (imageResult.IsFailure)
                {
                    Log.Error("Image creation failed: {Error}", imageResult.Error);
                    return BadRequest(imageResult.Error);
                }
                image = imageResult.Value;
            }

            var ingredients = request.Ingredients
                .Select(i => Ingredient.Create(
                    Guid.NewGuid(),
                    i.IngredientName,
                    i.WeightInGrams,
                    i.Calories,
                    i.Protein,
                    i.Fat,
                    i.Carbs).Value)
                .ToList();

            var kebabResult = Kebab.Create(
                Guid.NewGuid(),
                request.KebabName,
                request.KebabDescription,
                request.Price,
                request.Stuffing,
                request.Wrap,
                request.IsAvailable,
                image,
                ingredients,
                _nutritionCalculatorService);

            if (kebabResult.IsFailure)
            {
                Log.Error("Kebab creation failed: {Error}", kebabResult.Error);
                return BadRequest(kebabResult.Error);
            }

            var kebabId = await _kebabService.CreateKebab(kebabResult.Value, request.TitleImage, _staticFilesPath);

            Log.Information("Kebab created successfully with ID: {KebabId} and Name: {Name}", kebabId, request.KebabName);
            watch.Stop();
            Log.Information("Completed request to create a kebab by Id in {ElapsedMilliseconds}ms", watch.ElapsedMilliseconds);
            return Ok(new { Message = "Kebab created successfully", Id = kebabId, request.KebabName });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Internal server error");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<KebabResponse>>> GetAllKebabs()
    {
        var watch = Stopwatch.StartNew();
        Log.Information("Starting request to get all kebabs from the database");

        try
        {
            var kebabs = await _kebabService.GetAllKebabs();
            if (kebabs == null || !kebabs.Any())
            {
                Log.Warning("No kebabs found in the database");
                return Ok(new List<KebabResponse>()); // Возвращаем пустой список
            }

            var response = kebabs.Select(k => new KebabResponse(
                k.Id,
                k.KebabName,
                k.KebabDescription,
                k.Price,
                k.Stuffing,
                k.Wrap,
                k.IsAvailable,
                k.TitleImage?.Path ?? string.Empty,
                k.Ingredients.Select(i => new IngredientResponse(
                    i.Id,
                    i.IngredientName,
                    i.WeightInGrams,
                    i.Calories,
                    i.Protein,
                    i.Fat,
                    i.Carbs)).ToList(),
                k.Calories
            )).ToList();

            Log.Information("Successfully retrived {KebabCount} kebabs from the database", response.Count);
            watch.Stop();
            Log.Information("Completed request to get all kebabs in {ElapsedMilliseconds}ms", watch.ElapsedMilliseconds);
            return Ok(new { Message = "Kebabs retrieved successfully", response.Count, Data = response });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while getting all kebabs from database");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Kebab>> GetKebabById(Guid id)
    {
        var watch = Stopwatch.StartNew();
        Log.Information("Starting request to get a kebab by Id from the database with Id: {Id}", id);

        try
        {
            var kebab = await _kebabService.GetKebabById(id);
            if (kebab == null)
            {
                Log.Warning("Kebab with Id: {Id} not found", id);
                return NotFound();
            }

            var response = new KebabResponse(
                kebab.Id,
                kebab.KebabName,
                kebab.KebabDescription,
                kebab.Price,
                kebab.Stuffing,
                kebab.Wrap,
                kebab.IsAvailable,
                kebab.TitleImage?.Path ?? string.Empty,
                kebab.Ingredients.Select(i => new IngredientResponse(
                    i.Id,
                    i.IngredientName,
                    i.WeightInGrams,
                    i.Calories,
                    i.Protein,
                    i.Fat,
                    i.Carbs)).ToList(),
                kebab.Calories);

            Log.Information("Successfully retrived kebab with {Id} and Name: {KebabName} from the database", id, kebab.KebabName);
            watch.Stop();
            Log.Information("Completed request to get a kebab by Id in {ElapsedMilliseconds}ms", watch.ElapsedMilliseconds);
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            Log.Warning("Kebab with Id: {Id} not found in the database", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while getting a kebab by Id from the database with Id: {Id}", id);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateKebab(Guid id, [FromForm] KebabRequest request)
    {
        var watch = Stopwatch.StartNew();
        Log.Information("Starting request to update a kebab with Id: {Id} and Name: {KebabName}", id, request.KebabName);

        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            Log.Warning("Validation failed for kebab with Id: {Id}. Errors: {Errors}", id, validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        try
        {
            Image? image = null;
            if (request.TitleImage != null)
            {
                var imageResult = await _imageService.CreateImage(request.TitleImage, _staticFilesPath);
                if (imageResult.IsFailure)
                {
                    Log.Error("Image creation failed for kebab with Id: {Id}. Error: {Error}", id, imageResult.Error);
                    return BadRequest(imageResult.Error);
                }
                image = imageResult.Value;
            }

            var ingredients = request.Ingredients
                .Select(i => Ingredient.Create(
                    Guid.NewGuid(),
                    i.IngredientName,
                    i.WeightInGrams,
                    i.Calories,
                    i.Protein,
                    i.Fat,
                    i.Carbs).Value)
                .ToList();

            var kebabResult = Kebab.Create(
                id,
                request.KebabName,
                request.KebabDescription,
                request.Price,
                request.Stuffing,
                request.Wrap,
                request.IsAvailable,
                image,
                ingredients,
                _nutritionCalculatorService);

            if (kebabResult.IsFailure)
            {
                Log.Error("Kebab update failed: {Error}", kebabResult.Error);
                return BadRequest(kebabResult.Error);
            }

            await _kebabService.UpdateKebab(kebabResult.Value, request.TitleImage?.FileName, request.TitleImage, _staticFilesPath);

            Log.Information("Kebab with Id: {Id} updated successfully", id);
            watch.Stop();
            Log.Information("Completed request to update kebab with Id: {Id} in {ElapsedMilliseconds}ms", id, watch.ElapsedMilliseconds);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            Log.Warning("Kebab with Id: {Id} not found in the database", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while updating kebab with Id: {Id}", id);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> DeleteKebab(Guid id)
    {
        var watch = Stopwatch.StartNew();
        Log.Information("Starting request to delete a kebab with Id: {Id}", id);

        try
        {
            var kebabId = await _kebabService.DeleteKebab(id);

            Log.Information("Kebab with Id: {Id} deleted successfully", id);
            watch.Stop();
            Log.Information("Completed request to delete kebab with Id: {Id} in {ElapsedMilliseconds}ms", id, watch.ElapsedMilliseconds);
            return Ok(new { Message = "Kebab deleted successfully", Id = kebabId });
        }
        catch (KeyNotFoundException ex)
        {
            Log.Warning("Kebab with Id: {Id} not found in the database", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while deleting kebab with Id: {Id}", id);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}