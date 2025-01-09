using KebabStoreGen2.Application.DTOs;
using KebabStoreGen2.Application.Services.Mappers;
using KebabStoreGen2.Core.Models.Enums;

namespace KebabStoreGen2.Application.DTOsp;

public class KebabDto
{
    public Guid Id { get; set; }
    public string KebabName { get; set; }
    public string KebabDescription { get; set; }
    public decimal KebabPrice { get; set; }
    public StuffingCategory Stuffing { get; set; }
    public WrapCategory Wrap { get; set; }
    public bool IsAvailable { get; set; }
    public ImageDto TitleImage { get; set; }
    public List<KebabIngredientDtoMapper> Ingredients { get; set; } // Список ингредиентов с весом для конкретного кебаба
    public int TotalWeight { get; set; }
    public decimal TotalCalories { get; set; }
    public decimal TotalCarbs { get; set; }
    public decimal TotalFat { get; set; }
    public decimal TotalProtein { get; set; }
}