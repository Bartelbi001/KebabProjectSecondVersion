using KebabStoreGen2.Application.DTOs;
using KebabStoreGen2.Application.DTOsp;
using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Models;

namespace KebabStoreGen2.Application.Services.Mappers;

public class KebabDtoMapper : IMapper<Kebab, KebabDto>, IMapper<KebabDto, Kebab>
{
    public KebabDto Map(Kebab kebab)
    {
        return new KebabDto
        {
            Id = kebab.Id,
            KebabName = kebab.KebabName,
            KebabDescription = kebab.KebabDescription,
            KebabPrice = kebab.KebabPrice,
            Stuffing = kebab.Stuffing,
            Wrap = kebab.Wrap,
            IsAvailable = kebab.IsAvailable,
            TitleImage = new ImageDto { FileName = kebab.TitleImage.FileName, Path = kebab.TitleImage.Path },
            Ingredients = kebab.Ingredients.Select(ingredient => new IngredientDto
            {
                Id = ingredient.Id,
                IngredientName = ingredient.IngredientName,
                CaloriesPer100g = ingredient.CaloriesPer100g,
                ProteinPer100g = ingredient.FatPer100g,
                FatPer100g = ingredient.FatPer100g,
                CarbsPer100g = ingredient.CarbsPer100g,
                SugarPer100g = ingredient.SugarPer100g,
                ContainsLactose = ingredient.ContainsLactose
            }).ToList(),
            TotalWeight = kebab.TotalWeight,
            TotalCalories = kebab.TotalCalories,
            TotalCarbs = kebab.TotalCarbs,
            TotalFat = kebab.TotalFat,
            TotalProtein = kebab.TotalProtein,
        };
    }

    public Kebab Map(KebabDto kebabDto)
    {
        var ingredients = kebabDto.Ingredients.Select(dto => Ingredient.Create(
            dto.Id,
            dto.IngredientName,
            dto.CaloriesPer100g,
            dto.ProteinPer100g,
            dto.FatPer100g,
            dto.CarbsPer100g,
            dto.SugarPer100g ?? 0,
            dto.ContainsLactose ?? false
        ).Value).ToList();

        return Kebab.Create(
            kebabDto.Id,
            kebabDto.KebabName,
            kebabDto.KebabDescription,
            kebabDto.KebabPrice,
            kebabDto.Stuffing,
            kebabDto.Wrap,
            kebabDto.IsAvailable,
            Image.Create(kebabDto.TitleImage.FileName, kebabDto.TitleImage.Path).Value,
            ingredients,
            new NutritionAndWeightCalculatorService(),
            kebabDto.Ingredients.Select(i => i.Weight).ToList()
        ).Value;
    }
}