using CSharpFunctionalExtensions;
using FluentValidation;
using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Contracts;
using KebabStoreGen2.Core.Models;
using Microsoft.AspNetCore.Mvc;

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
        try
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var imageResult = await _imagesService.CreateImage(request.TitleImage, _staticFilesPath);
            if (imageResult.IsFailure)
            {
                return BadRequest(imageResult.Error);
            }

            var kebabResult = Kebab.Create(Guid.NewGuid(), request.Name, request.Description, request.Price, imageResult.Value);
            if (kebabResult.IsFailure)
            {
                return BadRequest(kebabResult.Error);
            }

            var kebabId = await _kebabService.CreateKebab(kebabResult.Value);

            return Ok(kebabId);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<KebabsResponse>>> GetAllKebabs()
    {
        try
        {
            var kebabs = await _kebabService.GetAllKebabs();
            if (kebabs == null || !kebabs.Any())
            {
                return Ok(new List<KebabsResponse>()); // Возвращаем пустой список
            }

            var response = kebabs.Select(k => new KebabsResponse(
                k.Id,
                k.Name,
                k.Description,
                k.Price,
                k.TitleImage != null ? k.TitleImage.Path : string.Empty // Проверка на null
            )).ToList();

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Kebab>> GetKebabById(Guid id)
    {
        try
        {
            var result = await _kebabService.GetKebabById(id);
            if (result == null)
            {
                return NotFound("Kebab not found");
                throw new KeyNotFoundException();
            }

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Kebab not found");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateKebab(Guid id, [FromForm] KebabsRequest request)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var imageResult = await _imagesService.CreateImage(request.TitleImage, _staticFilesPath);
            if (imageResult.IsFailure)
            {
                return BadRequest(imageResult.Error);
            }

            var kebabId = await _kebabService.UpdateKebab(id, request.Name, request.Description, request.Price, imageResult.Value.Path);

            return Ok(kebabId);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Kebab not found");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> DeleteKebab(Guid id)
    {
        try
        {
            var result = await _kebabService.DeleteKebab(id);

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Kebab not found");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}