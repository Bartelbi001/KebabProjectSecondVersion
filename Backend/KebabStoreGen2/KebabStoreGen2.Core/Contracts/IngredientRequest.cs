using KebabStoreGen2.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace KebabStoreGen2.Core.Contracts;

public record IngredientRequest(
    [Required][MaxLength(IngredientConstants.MAX_INGREDIENTNAME_LENGTH)] string IngredientName,
    [Required] int WeightInGrams,
    [Required] int Calories,
    [Required] int Protein,
    [Required] int Fat,
    [Required] int Carbs);