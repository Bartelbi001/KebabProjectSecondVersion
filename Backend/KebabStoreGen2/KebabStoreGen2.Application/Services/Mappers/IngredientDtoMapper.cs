using KebabStoreGen2.Application.DTOs;
using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Models;

namespace KebabStoreGen2.Application.Services.Mappers;

public class IngredientDtoMapper : IMapper<Ingredient, IngredientDto>, IMapper<IngredientDto, Ingredient>
{
    public IngredientDto Map(Ingredient ingredient)
    {

        return new IngredientDto
        {
            Id = ingredient.Id,
            IngredientName = ingredient.IngredientName,
            CaloriesPer100g = ingredient.CaloriesPer100g,
            ProteinPer100g = ingredient.ProteinPer100g,
            FatPer100g = ingredient.FatPer100g,
            CarbsPer100g = ingredient.CarbsPer100g,
            SugarPer100g = ingredient.SugarPer100g,
            ContainsLactose = ingredient.ContainsLactose,
        };
    }

    public Ingredient Map(IngredientDto ingredientDto)
    {
        return Ingredient.Create(
            ingredientDto.Id,
            ingredientDto.IngredientName,
            ingredientDto.CaloriesPer100g,
            ingredientDto.ProteinPer100g,
            ingredientDto.FatPer100g,
            ingredientDto.CarbsPer100g,
            ingredientDto.SugarPer100g ?? 0,
            ingredientDto.ContainsLactose ?? false
        ).Value;
    }
}