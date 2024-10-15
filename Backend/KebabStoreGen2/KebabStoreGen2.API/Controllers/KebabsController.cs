using KebabStoreGen2.API.Contracts;
using KebabStoreGen2.API.KebabStoreGen2.Core.Models;
using KebabStoreGen2.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace KebabStoreGen2.API.Controllers;

[ApiController]
[Route("[controller]")]
public class KebabsController : ControllerBase
{
    private readonly string _staticFilesPath =
        Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles/Images");

    private readonly KebabsService _kebabsService;
    private readonly ImagesService _imagesService;

    public KebabsController(KebabsService kebabsService, ImagesService imagesService)
    {
        _kebabsService = kebabsService;
        _imagesService = imagesService;
    }

    public ImagesService ImagesService { get; }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateKebab([FromBody] KebabsRequest request)
    {
        var imageResult = await _imagesService.CreateImage(
            request.TitleImage,
            _staticFilesPath);

        if (imageResult.IsFailure)
        {
            return BadRequest(imageResult.Error);
        }

        var kebabResult = Kebab.Create(
            Guid.NewGuid(),
            request.Name,
            request.Description,
            request.Price,
            imageResult.Value);

        if (kebabResult.IsFailure)
        {
            return BadRequest(imageResult.Error);
        }

        var kebabId = await _kebabsService.CreateKebab(kebabResult.Value);

        return Ok(kebabId);
    }

    [HttpGet]
    public async Task<ActionResult<List<Kebab>>> GetAllKebabs()
    {
        var kebabs = await _kebabsService.GetAllKebabs();

        var response = kebabs.Select(k => new KebabsResponse(k.Id, k.Name, k.Description, k.Price, k.TitleImage.Path));

        return Ok(response);
    }
}
