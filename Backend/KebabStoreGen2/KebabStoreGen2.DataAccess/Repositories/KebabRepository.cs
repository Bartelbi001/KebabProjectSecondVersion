using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace KebabStoreGen2.DataAccess.Repositories;

public class KebabRepository : IKebabRepository
{
    private readonly KebabStoreGen2DbContext _context;

    public KebabRepository(KebabStoreGen2DbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Create(KebabEntity kebabEntity)
    {
        await _context.KebabEntities.AddAsync(kebabEntity);
        await _context.SaveChangesAsync();

        return kebabEntity.Id;
    }

    public async Task<KebabEntity> GetById(Guid id)
    {
        return await _context.KebabEntities
            .Include(k => k.KebabIngredients)
                .ThenInclude(ki => ki.Ingredient)
            .AsNoTracking()
            .FirstOrDefaultAsync(k => k.Id == id);
    }

    public async Task<List<KebabEntity>> GetAll()
    {
        return await _context.KebabEntities
            .Include(k => k.KebabIngredients)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task Update(KebabEntity kebabEntity)
    {
        var existingKebab = await _context.KebabEntities
            .Include(k => k.KebabIngredients)
            .FirstOrDefaultAsync(k => k.Id == kebabEntity.Id);

        if (existingKebab == null)
        {
            throw new KeyNotFoundException("Kebab not found");
        }

        _context.Entry(existingKebab).CurrentValues.SetValues(kebabEntity);

        existingKebab.KebabIngredients.Clear();
        existingKebab.KebabIngredients.AddRange(kebabEntity.KebabIngredients);

        await _context.SaveChangesAsync();
    }

    public async Task<Guid> Delete(Guid id)
    {
        var kebabEntity = await _context.KebabEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(k => k.Id == id);

        if (kebabEntity == null)
        {
            throw new KeyNotFoundException("Kebab not found");
        }

        _context.KebabEntities.Remove(kebabEntity);
        await _context.SaveChangesAsync();

        return kebabEntity.Id;
    }
}