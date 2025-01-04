namespace KebabStoreGen2.Core.Contracts;

public class IngredientResponse
{
    public Guid Id { get; set; }
    public required string IngredientName { get; set; }
    public required decimal CaloriesPer100g { get; set; }
    public required decimal ProteinPer100g { get; set; }
    public required decimal FatPer100g { get; set; }
    public required decimal CarbsPer100g { get; set; }
    public decimal? SugarPer100g { get; set; }
    public bool? ContainsLactose { get; set; }
}