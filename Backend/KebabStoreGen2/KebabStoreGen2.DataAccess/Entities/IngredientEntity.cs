using KebabStoreGen2.DataAccess.LinkingModels;

namespace KebabStoreGen2.DataAccess.Entities;

public class IngredientEntity
{
    public Guid Id { get; set; }
    public string IngredientName { get; set; } = string.Empty;
    public decimal CaloriesPer100g { get; set; }
    public decimal ProteinPer100g { get; set; }
    public decimal FatPer100g { get; set; }
    public decimal CarbsPer100g { get; set; }
    public decimal? SugarPer100g { get; set; }
    public bool? ContainsLactose { get; set; }
    public List<KebabIngredient> KebabIngredients { get; set; } = new();
}