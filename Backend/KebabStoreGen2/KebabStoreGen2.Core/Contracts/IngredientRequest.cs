using KebabStoreGen2.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace KebabStoreGen2.Core.Contracts;

public record IngredientRequest(
    [Required][MaxLength(IngredientConstants.MAX_INGREDIENTNAME_LENGTH)] string IngredientName,
    [Required] decimal CaloriesPer100g,
    [Required] decimal ProteinPer100g,
    [Required] decimal FatPer100g,
    [Required] decimal CarbsPer100g,
    decimal? SugarPer100g = null,
    bool? ContainsLactose = null
);