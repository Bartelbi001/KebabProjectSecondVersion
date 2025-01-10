using KebabStoreGen2.Core.Models.Enums;
using KebabStoreGen2.DataAccess.LinkingModels;

namespace KebabStoreGen2.DataAccess.Entities;

public class KebabEntity
{
    public Guid Id { get; set; }
    public string KebabName { get; set; } = string.Empty;
    public string KebabDescription { get; set; } = string.Empty;
    public decimal KebabPrice { get; set; }
    public StuffingCategory Stuffing { get; set; }
    public WrapCategory Wrap { get; set; }
    public bool IsAvailable { get; set; }
    public string? TitleImagePath { get; set; } // Используем строку для пути к изображению
    public List<KebabIngredient>? KebabIngredients { get; set; }
    public int TotalWeight { get; set; }
    public decimal TotalCalories { get; set; }
    public decimal TotalCarbs { get; set; }
    public decimal TotalFat { get; set; }
    public decimal TotalProtein { get; set; }
}