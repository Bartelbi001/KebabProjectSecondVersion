using FluentValidation;
using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Contracts;
using KebabStoreGen2.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;

namespace KebabStoreGen2.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientController : ControllerBase
{
    private readonly IIngredientService _ingredientService;
    private readonly IValidator<IngredientRequest> _validator;

    public IngredientController(IIngredientService ingredientService, IValidator<IngredientRequest> validator)
    {
        _ingredientService = ingredientService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateIngredient([FromForm] IngredientRequest request)
    {
        var watch = Stopwatch.StartNew();
        Log.Information("Creating a new ingredient with Name: {Name}", request.IngredientName);

        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            Log.Warning("Validation failed: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var ingredientResult = Ingredient.Create(
                Guid.NewGuid(),
                request.IngredientName,
                request.CaloriesPer100g,
                request.ProteinPer100g,
                request.FatPer100g,
                request.CarbsPer100g,
                request.SugarPer100g ?? 0,
                request.ContainsLactose ?? false
            );
            if (ingredientResult.IsFailure)
            {
                Log.Error("Ingredient creation failed: {Error}", ingredientResult.Error);
                return BadRequest(ingredientResult.Error);
            }

            var ingredientId = await _ingredientService.CreateIngredient(ingredientResult.Value);

            Log.Information("Ingredient created successfully with ID: {IngredientId} and Name: {Name}", ingredientId, request.IngredientName);
            watch.Stop();
            Log.Information("Completed request to create an ingredient by Id in {ElapsedMilliseconds}ms", watch.ElapsedMilliseconds);
            return CreatedAtAction(nameof(GetIngredientById), new { id = ingredientId }, new { id = ingredientId });
            //return Ok(new { Message = "Ingredient created successfully", Id = ingredientId, request.IngredientName });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Internal server error");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<IngredientResponse>>> GetAllIngredients()
    {
        var watch = Stopwatch.StartNew();
        Log.Information("Starting request to get all ingredients from the database");

        try
        {
            var ingredients = await _ingredientService.GetAllIngredients();
            if (ingredients == null || !ingredients.Any())
            {
                Log.Warning("No ingredients found in the database");
                return Ok(new List<IngredientResponse>());
            }

            var response = ingredients.Select(i => new IngredientResponse(
                i.Id,
                i.IngredientName,
                i.CaloriesPer100g,
                i.ProteinPer100g,
                i.FatPer100g,
                i.CarbsPer100g,
                i.SugarPer100g,
                i.ContainsLactose
            )).ToList();

            Log.Information("Successfully retrieved {IngredientCount} ingredients from the database", response.Count);
            watch.Stop();
            Log.Information("Completed request to get all ingredients in {ElapsedMilliseconds}ms", watch.ElapsedMilliseconds);
            return Ok(new { Message = "Ingredients retrieved successfully", response.Count, Data = response });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while getting all ingredients from database");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<IngredientResponse>> GetIngredientById(Guid id)
    {
        var watch = Stopwatch.StartNew();
        Log.Information("Starting request to get an ingredient by Id from the database with Id: {Id}", id);

        try
        {
            var ingredient = await _ingredientService.GetIngredientById(id);
            if (ingredient == null)
            {
                Log.Warning("Ingredient with Id: {Id} not found in the database", id);
                return NotFound();
            }

            var response = new IngredientResponse(
                ingredient.Id,
                ingredient.IngredientName,
                ingredient.CaloriesPer100g,
                ingredient.ProteinPer100g,
                ingredient.FatPer100g,
                ingredient.CarbsPer100g,
                ingredient.SugarPer100g,
                ingredient.ContainsLactose
            );

            Log.Information("Successfully retrieved ingredient with Id: {Id} and Name: {IngredientName} from the database", id, ingredient.IngredientName);
            watch.Stop();
            Log.Information("Completed request to get an ingredient by Id in {ElapsedMilliseconds}ms", watch.ElapsedMilliseconds);
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            Log.Warning("Ingredient with Id: {Id} not found", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while getting an ingredient by Id from the database with Id: {Id}", id);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateIngredient(Guid id, [FromForm] IngredientRequest request)
    {
        var watch = new Stopwatch();
        Log.Information("Starting request to update an ingredient with Id: {Id} and Name: {IngredientName}", id, request.IngredientName);

        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            Log.Warning("Validation failed for ingredient with Id: {Id}. Errors: {Errors}", id, validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        try
        {
            await _ingredientService.UpdateIngredient(
                id,
                request.IngredientName,
                request.CaloriesPer100g,
                request.ProteinPer100g,
                request.FatPer100g,
                request.CarbsPer100g,
                request.SugarPer100g ?? 0,
                request.ContainsLactose ?? false
            );

            Log.Information("Ingredient with Id: {Id} updated successfully", id);
            watch.Stop();
            Log.Information("Completed request to update ingredient with Id: {Id} in {ElapsedMilliseconds}ms", id, watch.ElapsedMilliseconds);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            Log.Warning("Ingredient with Id: {Id} not found", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while updating ingredient with Id: {Id}", id);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> DeleteIngredient(Guid id)
    {
        var watch = Stopwatch.StartNew();
        Log.Information("Starting request to delete an ingredient with Id: {Id}", id);

        try
        {
            var ingredientId = await _ingredientService.DeleteIngredient(id);

            Log.Information("Ingredient with Id: {Id} deleted successfully", id);
            watch.Stop();
            Log.Information("Completed request to delete ingredient with Id: {Id} in {ElapsedMilliseconds}ms", id, watch.ElapsedMilliseconds);
            return Ok(new { Message = "Ingredient deleted successfully", Id = ingredientId });
        }
        catch (KeyNotFoundException ex)
        {
            Log.Warning("Ingredient with Id: {Id} not found", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while deleting ingredient with Id: {Id}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}