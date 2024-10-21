using CSharpFunctionalExtensions;
using FluentValidation;
using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Contracts;
using KebabStoreGen2.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace KebabStoreGen2.API.Controllers;

[ApiController]
[Route("[controller]")]
public class KebabsController : ControllerBase
{
    private readonly string _staticFilesPath =
        Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles/Images");

    private readonly IKebabService _kebabService;
    private readonly IImageService _imagesService;
    private readonly IValidator<KebabsRequest> _validator;

    public KebabsController(IKebabService kebabService, IImageService imagesService, IValidator<KebabsRequest> validator)
    {
        _kebabService = kebabService;
        _imagesService = imagesService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateKebab([FromForm] KebabsRequest request)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        Log.Information("Creating a new kebab with Name: {Name}", request.Name);

        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            Log.Warning("Validation failed: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var imageResult = await _imagesService.CreateImage(request.TitleImage, _staticFilesPath);
            if (imageResult.IsFailure)
            {
                Log.Error("Image creation failed: {Error}", imageResult.Error);
                return BadRequest(imageResult.Error);
            }

            var kebabResult = Kebab.Create(Guid.NewGuid(), request.Name, request.Description, request.Price, imageResult.Value);
            if (kebabResult.IsFailure)
            {
                Log.Error("Kebab creation failed: {Error}", kebabResult.Error);
                return BadRequest(kebabResult.Error);
            }

            var kebabId = await _kebabService.CreateKebab(kebabResult.Value);

            Log.Information("Kebab created successfully with ID: {KebabId} and Name: {Name}", kebabId, request.Name);
            watch.Stop();
            Log.Information("Completed request to create a kebab by Id in {ElapsedMilliseconds}ms", watch.ElapsedMilliseconds);
            return Ok(new { Message = "Kebab created successfully", Id = kebabId, request.Name });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Internal server error");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<KebabsResponse>>> GetAllKebabs()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        Log.Information("Starting request to get all kebabs from the database");

        try
        {
            var kebabs = await _kebabService.GetAllKebabs();
            if (kebabs == null || !kebabs.Any())
            {
                Log.Warning("No kebabs found in the database");
                return Ok(new List<KebabsResponse>()); // Возвращаем пустой список
            }

            var response = kebabs.Select(k => new KebabsResponse(
                k.Id,
                k.Name,
                k.Description,
                k.Price,
                k.TitleImage != null ? k.TitleImage.Path : string.Empty // Проверка на null
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
        var watch = System.Diagnostics.Stopwatch.StartNew();
        Log.Information("Starting request to get a kebab by Id from the database with Id: {Id}", id);

        try
        {
            var result = await _kebabService.GetKebabById(id);

            Log.Information("Successfully retrived kebab with {Id} and Name: {KebabName} from the database", id, result.Name);
            watch.Stop();
            Log.Information("Completed request to get a kebab by Id in {ElapsedMilliseconds}ms", watch.ElapsedMilliseconds);
            return Ok(new { Message = "Kebab retrieved successfully", Id = id, result.Name });
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
    public async Task<ActionResult<Guid>> UpdateKebab(Guid id, [FromForm] KebabsRequest request)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        Log.Information("Starting request to update a kebab with Id: {Id} and Name: {KebabName}", id, request.Name);

        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            Log.Warning("Validation failed for kebab with Id: {Id}. Errors: {Errors}", id, validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var imageResult = await _imagesService.CreateImage(request.TitleImage, _staticFilesPath);
            if (imageResult.IsFailure)
            {
                Log.Error("Image creation failed for kebab with Id: {Id}. Error: {Error}", id, imageResult.Error);
                return BadRequest(imageResult.Error);
            }

            var kebabId = await _kebabService.UpdateKebab(id, request.Name, request.Description, request.Price, imageResult.Value.Path);

            Log.Information("Kebab with Id: {Id} updated successfully", kebabId);
            watch.Stop();
            Log.Information("Completed request to update kebab with Id: {Id} in {ElapsedMilliseconds}ms", kebabId, watch.ElapsedMilliseconds);
            return Ok(new { Message = "Kebab updated successfully", Id = kebabId });
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
        var watch = System.Diagnostics.Stopwatch.StartNew();
        Log.Information("Starting request to delete a kebab with Id: {Id}", id);

        try
        {
            var result = await _kebabService.DeleteKebab(id);
            Log.Information("Kebab with Id: {Id} deleted successfully", id);

            watch.Stop();
            Log.Information("Completed request to delete kebab with Id: {Id} in {ElapsedMilliseconds}ms", id, watch.ElapsedMilliseconds);

            return Ok(new { Message = "Kebab deleted successfully", Id = result });
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