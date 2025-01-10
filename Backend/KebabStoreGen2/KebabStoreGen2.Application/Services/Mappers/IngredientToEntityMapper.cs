using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Models;
using KebabStoreGen2.DataAccess.Entities;

namespace KebabStoreGen2.Application.Services.Mappers
{
    public class IngredientToEntityMapper : IMapper<Ingredient, IngredientEntity>
    {
        public IngredientEntity Map(Ingredient ingredient)
        {
            return new IngredientEntity
            {
                Id = ingredient.Id,
                IngredientName = ingredient.IngredientName,
                CaloriesPer100g = ingredient.CaloriesPer100g,
                ProteinPer100g = ingredient.ProteinPer100g,
                FatPer100g = ingredient.FatPer100g,
                CarbsPer100g = ingredient.CarbsPer100g,
                SugarPer100g = ingredient.SugarPer100g ?? 0,
                ContainsLactose = ingredient.ContainsLactose ?? false
            };
        }
    }
}