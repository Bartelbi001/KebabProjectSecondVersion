using KebabStoreGen2.Core.Models.Enums;

namespace KebabStoreGen2.Core.Contracts;

public class KebabResponse
{
    public Guid Id { get; set; }
    public string KebabName { get; set; } = string.Empty;
    public string KebabDescription { get; set; } = string.Empty;
    public decimal KebabPrice { get; set; }
    public StuffingCategory Stuffing { get; set; }
    public WrapCategory Wrap { get; set; }
    public bool IsAvailable { get; set; }
    public List<KebabIngredientResponse> Ingredients { get; set; } = new();
    public decimal TotalCalories { get; set; }
    public decimal TotalCarbs { get; set; }
    public decimal TotalFat { get; set; }
    public decimal TotalProtein { get; set; }
    public decimal TotalSugar { get; set; }
    public int TotalWeight { get; set; }
    public string? TitleImagePath { get; set; }
}