using KebabStoreGen2.DataAccess.Entities;

namespace KebabStoreGen2.Core.Models.LinkingModels;

public class KebabIngredient
{
    public Guid KebabId { get; set; }
    public KebabEntity Kebab { get; set; } = null!;

    public Guid IngredientId { get; set; }
    public IngredientEntity Ingredient { get; set; } = null!;

    public int Weight { get; set; } // Вес конкретного ингредиента в данном кебабе
}