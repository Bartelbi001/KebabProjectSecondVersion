using KebabStoreGen2.Application.DTOs;
using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Models;
using KebabStoreGen2.DataAccess.Entities;
using KebabStoreGen2.DataAccess.LinkingModels;

namespace KebabStoreGen2.Application.Services.Mappers;

public class KebabIngredientDtoMapper : IMapper<KebabIngredient, KebabIngredientDto>, IMapper<KebabIngredientDto, KebabIngredient>
{
    private readonly IMapper<IngredientDto, Ingredient> _ingredientDtoToIngredientMapper;
    private readonly IMapper<Ingredient, IngredientEntity> _ingredientToIngredientEntityMapper;
    private readonly IMapper<IngredientEntity, IngredientDto> _ingredientEntityToIngredientDtoMapper;

    public KebabIngredientDtoMapper(IMapper<IngredientDto, Ingredient> ingredientDtoToIngredientMapper,
                                    IMapper<Ingredient, IngredientEntity> ingredientToIngredientEntityMapper,
                                    IMapper<IngredientEntity, IngredientDto> ingredientEntityToIngredientDtoMapper)
    {
        _ingredientDtoToIngredientMapper = ingredientDtoToIngredientMapper;
        _ingredientToIngredientEntityMapper = ingredientToIngredientEntityMapper;
        _ingredientEntityToIngredientDtoMapper = ingredientEntityToIngredientDtoMapper;
    }
    public KebabIngredientDto Map(KebabIngredient kebabIngredient)
    {
        return new KebabIngredientDto
        {
            KebabId = kebabIngredient.KebabId,
            IngredientId = kebabIngredient.IngredientId,
            //Теперь используем правильный маппер
            Ingredient = _ingredientEntityToIngredientDtoMapper.Map(kebabIngredient.Ingredient),
            Weight = kebabIngredient.Weight
        };
    }
    public KebabIngredient Map(KebabIngredientDto kebabIngredientDto)
    {
        return new KebabIngredient
        {
            KebabId = kebabIngredientDto.KebabId,
            IngredientId = kebabIngredientDto.IngredientId,
            Ingredient = _ingredientToIngredientEntityMapper.Map(_ingredientDtoToIngredientMapper.Map(kebabIngredientDto.Ingredient)),
            Weight = kebabIngredientDto.Weight
        };
    }
}