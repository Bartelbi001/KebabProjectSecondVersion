using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Models;
using KebabStoreGen2.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace KebabStoreGen2.DataAccess.Repositories;

public class KebabRepository : IKebabsRepository
{
    private readonly KebabStoreGen2DbContext _context;

    public KebabRepository(KebabStoreGen2DbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Create(Kebab kebab)
    {
        var kebabEntity = new KebabEntity
        {
            Id = kebab.Id,
            KebabName = kebab.KebabName,
            KebabDescription = kebab.KebabDescription,
            Price = kebab.Price,
            TitleImagePath = kebab.TitleImage?.Path // Используем Path для хранения пути
        };

        await _context.AddAsync(kebabEntity);
        await _context.SaveChangesAsync();

        return kebabEntity.Id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        var kebab = await _context.KebabEntities.FindAsync(id);

        if (kebab == null)
        {
            throw new KeyNotFoundException("Kebab not found");
        }

        await _context.KebabEntities
            .Where(k => k.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }

    public async Task<Kebab> Get(Guid id)
    {
        var kebabEntity = await _context.KebabEntities.FindAsync(id);

        if (kebabEntity == null)
        {
            throw new KeyNotFoundException("Kebab not found");
        }

        var titleImage = !string.IsNullOrEmpty(kebabEntity.TitleImagePath) ? Image.Create(Path.GetFileName(kebabEntity.TitleImagePath), kebabEntity.TitleImagePath).Value : null;

        return Kebab.Create(kebabEntity.Id, kebabEntity.KebabName, kebabEntity.KebabDescription, kebabEntity.Price, titleImage).Value;
    }

    public async Task<List<Kebab>> GetAll()
    {
        var kebabsEntities = await _context.KebabEntities
            .AsNoTracking()
            .ToListAsync();

        var kebabs = kebabsEntities
            .Select(k =>
            {
                var titleImage = !string.IsNullOrEmpty(k.TitleImagePath) ? Image.Create(Path.GetFileName(k.TitleImagePath), k.TitleImagePath).Value : null;

                return Kebab.Create(k.Id, k.KebabName, k.KebabDescription, k.Price, titleImage).Value;
            })
            .ToList();

        return kebabs;
    }

    public async Task<Guid> Update(Guid id, string name, string description, decimal price, string? titleImagePath = null)
    {
        var kebab = await _context.KebabEntities.FindAsync(id);

        if (kebab == null)
        {
            throw new KeyNotFoundException("Kebab not found");
        }

        kebab.KebabName = name;
        kebab.KebabDescription = description;
        kebab.Price = price;

        if (!string.IsNullOrEmpty(titleImagePath))
        {
            kebab.TitleImagePath = titleImagePath;
        }

        _context.KebabEntities.Update(kebab);
        await _context.SaveChangesAsync();

        return id;
    }
}
