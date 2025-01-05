using KebabStoreGen2.Core.Constants;
using KebabStoreGen2.Core.Models.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace KebabStoreGen2.Core.Contracts;

public class KebabRequest
{
    public Guid Id { get; set; }

    [MaxLength(KebabConstants.MAX_KEBABNAME_LENGTH)]
    public required string KebabName { get; set; } = string.Empty;

    [MaxLength(KebabConstants.MAX_KEBABDESCRIPTION_LENGTH)]
    public required string KebabDescription { get; set; } = string.Empty;

    public required decimal KebabPrice { get; set; }

    public required StuffingCategory Stuffing { get; set; }

    public required WrapCategory Wrap { get; set; }

    public required bool IsAvailable { get; set; }

    public IFormFile? TitleImage { get; set; }

    public List<KebabIngredientRequest> Ingredients { get; set; } = new();
}