namespace KebabStoreGen2.Application.DTOs;

public class IngredientDto
{
    public Guid Id { get; set; }
    public string IngredientName { get; set; }
    public decimal CaloriesPer100g { get; set; }
    public decimal ProteinPer100g { get; set; }
    public decimal FatPer100g { get; set; }
    public decimal CarbsPer100g { get; set; }
    public decimal? SugarPer100g { get; set; }
    public bool? ContainsLactose { get; set; }
}