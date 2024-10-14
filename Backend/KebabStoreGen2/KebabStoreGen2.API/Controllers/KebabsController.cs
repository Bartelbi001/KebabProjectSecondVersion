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
    public async Task<ActionResult> CreateKebab(KebabsRequest request)
    {
        var imageResult = await _imagesService.CreateImage(request.TitleImage, _staticFilesPath);

        if (imageResult.IsFailure)
        {
            return BadRequest(imageResult.Error);
        }

        var kebab = Kebab.Create(Guid.NewGuid(), request.Name, request.Description, request.Price, imageResult.Value);

        if (kebab.IsFailure)
        {
            return BadRequest(imageResult.Error);
        }

        return Ok(kebab);
    }
}
