using KebabStoreGen2.DataAccess.Entities;

namespace KebabStoreGen2.Core.Abstractions;

public interface IIngredientRepository
{
    Task<Guid> Create(IngredientEntity ingredientEntity);
    Task<IngredientEntity> GetById(Guid id);
    Task<List<IngredientEntity>> GetAll();
    Task Update(IngredientEntity ingredientEntity);
    Task<Guid> Delete(Guid id);
}