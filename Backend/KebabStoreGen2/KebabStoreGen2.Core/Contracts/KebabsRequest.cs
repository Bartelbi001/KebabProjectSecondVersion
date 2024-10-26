using KebabStoreGen2.Core.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace KebabStoreGen2.Core.Contracts;

public record KebabsRequest(
    [Required][MaxLength(Kebab.MAX_KEBABNAME_LENGTH)] string Name,
    [Required][MaxLength(Kebab.MAX_KEBABDESCRIPTION_LENGTH)] string Description,
    [Required] decimal Price,
    IFormFile TitleImage);