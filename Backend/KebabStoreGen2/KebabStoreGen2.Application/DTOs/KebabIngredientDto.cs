namespace KebabStoreGen2.Application.DTOs;

public class KebabIngredientDto
{
    public Guid KebabId { get; set; }
    public Guid IngredientId { get; set; }
    public IngredientDto Ingredient { get; set; }
    public int Weight { get; set; } // Вес конкретного ингредиента в данном кебабе
}