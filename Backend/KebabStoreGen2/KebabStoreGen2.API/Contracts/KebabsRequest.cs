using KebabStoreGen2.API.KebabStoreGen2.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace KebabStoreGen2.API.Contracts;

public record KebabsRequest(
    [Required][MaxLength(Kebab.MAX_NAME_LENGTH)] string Name,
    [Required][MaxLength(Kebab.MAX_DESCRIPTION_LENGTH)] string Description,
    [Required] decimal Price,
    IFormFile TitleImage);