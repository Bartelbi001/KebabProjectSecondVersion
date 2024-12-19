namespace KebabStoreGen2.Core.Contracts;

public class KebabIngredientResponse
{
    public Guid Id { get; set; }
    public string IngredientName { get; set; } = string.Empty; public int Weight { get; set; }
    public decimal CaloriesPer100g { get; set; }
    public decimal ProteinPer100g { get; set; }
    public decimal FatPer100g { get; set; }
    public decimal CarbsPer100g { get; set; }
    public decimal? SugarPer100g { get; set; }
    public bool? ContainsLactose { get; set; }
}
