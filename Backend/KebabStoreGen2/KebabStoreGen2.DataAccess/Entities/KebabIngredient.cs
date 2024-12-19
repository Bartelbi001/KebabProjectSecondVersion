namespace KebabStoreGen2.DataAccess.Entities
{
    public class KebabIngredient
    {
        public Guid KebabId { get; set; }
        public KebabEntity Kebab { get; set; } = null!;

        public Guid IngredientId { get; set; }
        public IngredientEntity Ingredient { get; set; } = null!;

        public int Weight { get; set; } // Вус конкретного ингредиента в данном кебабе
    }
}
