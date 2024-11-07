namespace KebabStoreGen2.DataAccess.Entities;

public class IngredientEntity
{
    public Guid Id { get; set; }
    public string IngredientName { get; set; } = string.Empty;
    public int WeightInGrams { get; set; }
    public int Calories { get; set; }
    public int Protein { get; set; }
    public int Fat { get; set; }
    public int Carbs { get; set; }
}
