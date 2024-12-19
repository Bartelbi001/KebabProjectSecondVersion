using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace KebabStoreGen2.DataAccess.Repositories;

public class IngredientRepository : IIngredientRepository
{
    private readonly KebabStoreGen2DbContext _context;

    public IngredientRepository(KebabStoreGen2DbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Create(IngredientEntity ingredientEntity)
    {
        ingredientEntity.Id = Guid.NewGuid();

        await _context.AddAsync(ingredientEntity);
        await _context.SaveChangesAsync();

        return ingredientEntity.Id;
    }

    public async Task<IngredientEntity> GetById(Guid id)
    {
        return await _context.IngredientEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<IngredientEntity>> GetAll()
    {
        return await _context.IngredientEntities
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task Update(IngredientEntity ingredientEntity)
    {
        var ingredient = await _context.IngredientEntities.FindAsync(ingredientEntity.Id);
        if (ingredient == null)
        {
            throw new KeyNotFoundException("Ingredient not found");
        }

        _context.Entry(ingredient).CurrentValues.SetValues(ingredientEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<Guid> Delete(Guid id)
    {
        var ingredientEntity = await _context.IngredientEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);

        if (ingredientEntity == null)
        {
            throw new KeyNotFoundException("Ingredient not found");
        }

        _context.IngredientEntities.Remove(ingredientEntity);
        await _context.SaveChangesAsync();

        return ingredientEntity.Id;
    }
}