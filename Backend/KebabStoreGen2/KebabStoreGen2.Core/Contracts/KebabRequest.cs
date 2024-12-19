using KebabStoreGen2.Core.Constants;
using KebabStoreGen2.Core.Models.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace KebabStoreGen2.Core.Contracts;

public class KebabRequest
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(KebabConstants.MAX_KEBABNAME_LENGTH)]
    public string KebabName { get; set; } = string.Empty;

    [Required]
    [MaxLength(KebabConstants.MAX_KEBABDESCRIPTION_LENGTH)]
    public string KebabDescription { get; set; } = string.Empty;

    [Required]
    public decimal KebabPrice { get; set; }

    [Required]
    public StuffingCategory Stuffing { get; set; }

    [Required]
    public WrapCategory Wrap { get; set; }

    [Required]
    public bool IsAvailable { get; set; }

    public IFormFile? TitleImage { get; set; }

    public List<KebabIngredientRequest> Ingredients { get; set; } = new();
}