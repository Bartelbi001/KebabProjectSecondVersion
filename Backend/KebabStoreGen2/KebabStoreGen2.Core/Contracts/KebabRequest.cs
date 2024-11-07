using KebabStoreGen2.Core.Constants;
using KebabStoreGen2.Core.Models.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace KebabStoreGen2.Core.Contracts;

public record KebabRequest(
    [Required][MaxLength(KebabConstants.MAX_KEBABNAME_LENGTH)] string KebabName,
    [Required][MaxLength(KebabConstants.MAX_KEBABDESCRIPTION_LENGTH)] string KebabDescription,
    [Required] decimal Price,
    [Required] StuffingCategory Stuffing,
    [Required] WrapCategory Wrap,
    [Required] bool IsAvailable,
    IFormFile? TitleImage,
    List<IngredientRequest> Ingredients);