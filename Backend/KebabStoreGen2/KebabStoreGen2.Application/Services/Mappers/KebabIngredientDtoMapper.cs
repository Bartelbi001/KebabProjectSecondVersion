using KebabStoreGen2.Application.DTOs;
using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Models;
using KebabStoreGen2.DataAccess.LinkingModels;

namespace KebabStoreGen2.Application.Services.Mappers;

public class KebabIngredientDtoMapper : IMapper<KebabIngredient, KebabIngredientDto>, IMapper<KebabIngredientDto, KebabIngredient>
{
    private readonly IMapper<Ingredient, IngredientDto> _ingredientMapper;
    public KebabIngredientDtoMapper(IMapper<Ingredient, IngredientDto> ingredientMapper)
    {
        _ingredientMapper = ingredientMapper;
    }
    public KebabIngredientDto Map(KebabIngredient kebabIngredient)
    {
        return new KebabIngredientDto
        {
            KebabId = kebabIngredient.KebabId,
            IngredientId = kebabIngredient.IngredientId,
            Ingredient = _ingredientMapper.Map(kebabIngredient.Ingredient),
            Weight = kebabIngredient.Weight
        };
    }
    public KebabIngredient Map(KebabIngredientDto kebabIngredientDto)
    {
        return new KebabIngredient
        {
            KebabId = kebabIngredientDto.KebabId,
            IngredientId = kebabIngredientDto.IngredientId,
            Ingredient = _ingredientMapper.Map(kebabIngredientDto.Ingredient),
            Weight = kebabIngredientDto.Weight
        };
    }
}