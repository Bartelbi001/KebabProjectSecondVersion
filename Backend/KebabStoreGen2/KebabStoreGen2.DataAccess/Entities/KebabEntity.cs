namespace KebabStoreGen2.DataAccess.Entities;

public class KebabEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? TitleImagePath { get; set; } // Используем строку для пути к изображению
}