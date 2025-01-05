using KebabStoreGen2.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace KebabStoreGen2.Core.Contracts;

public class IngredientRequest
{
    [MaxLength(IngredientConstants.MAX_INGREDIENTNAME_LENGTH)]
    public required string IngredientName { get; set; }
    public required decimal CaloriesPer100g { get; set; }
    public required decimal ProteinPer100g { get; set; }
    public required decimal FatPer100g { get; set; }
    public required decimal CarbsPer100g { get; set; }
    public decimal? SugarPer100g { get; set; }
    public bool? ContainsLactose { get; set; }
}