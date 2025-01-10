using KebabStoreGen2.Application.DTOs;
using KebabStoreGen2.Application.DTOsp;
using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Models;
using KebabStoreGen2.DataAccess.LinkingModels;

namespace KebabStoreGen2.Application.Services.Mappers;

public class KebabDtoMapper : IMapper<Kebab, KebabDto>, IMapper<KebabDto, Kebab>
{
    private readonly IMapper<KebabIngredient, KebabIngredientDto> _kebabIngredientMapper;
    public KebabDtoMapper(IMapper<KebabIngredient, KebabIngredientDto> kebabIngredientMapper)
    {
        _kebabIngredientMapper = kebabIngredientMapper;
    }
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
            Ingredients = kebab.Ingredients.Select(ingredient => _kebabIngredientMapper.Map(ingredient)).ToList(),
            TotalWeight = kebab.TotalWeight,
            TotalCalories = kebab.TotalCalories,
            TotalCarbs = kebab.TotalCarbs,
            TotalFat = kebab.TotalFat,
            TotalProtein = kebab.TotalProtein
        };
    }
    public Kebab Map(KebabDto kebabDto)
    {
        var ingredients = kebabDto.Ingredients.Select(dto => _kebabIngredientMapper.Map(dto)).ToList();
        return Kebab.Create(
            kebabDto.Id,
            kebabDto.KebabName,
            kebabDto.KebabDescription,
            kebabDto.KebabPrice,
            kebabDto.Stuffing,
            kebabDto.Wrap,
            kebabDto.IsAvailable,
            Image.Create(kebabDto.TitleImage.FileName, kebabDto.TitleImage.Path).Value,
            ingredients.Select(i => i.Ingredient).ToList(), // Передача только ингредиентов в Kebab
            new NutritionAndWeightCalculatorService(), // Вставьте ваш сервис расчета
            kebabDto.Ingredients.Select(i => i.Weight).ToList()
            ).Value;
    }
}