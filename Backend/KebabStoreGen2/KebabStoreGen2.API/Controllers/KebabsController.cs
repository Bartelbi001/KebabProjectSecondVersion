using KebabStoreGen2.API.Contracts;
using KebabStoreGen2.API.KebabStoreGen2.Core.Models;
using KebabStoreGen2.Core.Abstractions;
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

    public KebabsController(IKebabService kebabService, IImageService imagesService)
    {
        _kebabService = kebabService;
        _imagesService = imagesService;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateKebab([FromForm] KebabsRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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


    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateKebab(Guid id, [FromForm] KebabsRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
    public async Task<ActionResult<Guid>> KebabDelete(Guid id)
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