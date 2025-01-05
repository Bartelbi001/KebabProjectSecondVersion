using KebabStoreGen2.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace KebabStoreGen2.Core.Contracts;

public class KebabIngredientRequest
{
    public Guid? Id { get; set; } // Опционально для существующих и новых ингредиентов

    [MaxLength(IngredientConstants.MAX_INGREDIENTNAME_LENGTH)]
    public required string IngredientName { get; set; } = string.Empty;
    public required int Weight { get; set; }
    public required decimal CaloriesPer100g { get; set; }
    public required decimal ProteinPer100g { get; set; }
    public required decimal FatPer100g { get; set; }
    public required decimal CarbsPer100g { get; set; }
    public decimal? SugarPer100g { get; set; } // Nullable
    public bool? ContainsLactose { get; set; } // Nullable
}
