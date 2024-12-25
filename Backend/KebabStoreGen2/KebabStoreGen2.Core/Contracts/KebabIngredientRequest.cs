using KebabStoreGen2.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace KebabStoreGen2.Core.Contracts;

public class KebabIngredientRequest
{
    public Guid? Id { get; set; } // Опционально для существующих и новых ингредиентов

    [Required]
    [MaxLength(IngredientConstants.MAX_INGREDIENTNAME_LENGTH)]
    public string IngredientName { get; set; } = string.Empty;

    [Required]
    public int Weight { get; set; }

    [Required]
    public decimal CaloriesPer100g { get; set; }

    [Required]
    public decimal ProteinPer100g { get; set; }

    [Required]
    public decimal FatPer100g { get; set; }

    [Required]
    public decimal CarbsPer100g { get; set; }

    public decimal? SugarPer100g { get; set; } // Nullable

    public bool? ContainsLactose { get; set; } // Nullable
}
