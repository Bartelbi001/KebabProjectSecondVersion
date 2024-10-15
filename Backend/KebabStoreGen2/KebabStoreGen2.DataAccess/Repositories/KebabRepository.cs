using KebabStoreGen2.API.KebabStoreGen2.Core.Models;
using KebabStoreGen2.Core.Abstractions;
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
            Name = kebab.Name,
            Description = kebab.Description,
            Price = kebab.Price,
            TitleImagePath = kebab.TitleImage?.Path // Используем Path для хранения пути
        };

        await _context.AddAsync(kebabEntity);
        await _context.SaveChangesAsync();

        return kebabEntity.Id;
    }

    //public async Task<Guid> Create(Kebab kebab)
    //{
    //    var kebabEntity = new KebabEntity
    //    {
    //        Id = kebab.Id,
    //        Name = kebab.Name,
    //        Description = kebab.Description,
    //        Price = kebab.Price,
    //        TitleImage = kebab.TitleImage
    //    };

    //    await _context.AddAsync(kebabEntity);
    //    await _context.SaveChangesAsync();

    //    return kebabEntity.Id;
    //}

    public async Task<Guid> Delete(Guid id)
    {
        await _context.KebabEntities
            .Where(k => k.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }

    public async Task<List<Kebab>> Get()
    {
        var kebabsEntities = await _context.KebabEntities
            .AsNoTracking()
            .ToListAsync();

        var kebabs = kebabsEntities
            .Select(k =>
            {
                var titleImage = !string.IsNullOrEmpty(k.TitleImagePath) ? Image.Create(Path.GetFileName(k.TitleImagePath), k.TitleImagePath).Value : null;

                return Kebab.Create(k.Id, k.Name, k.Description, k.Price, titleImage).Value;
            })
            .ToList();

        return kebabs;
    }

    //public async Task<List<Kebab>> Get()
    //{
    //    var kebabsEntities = await _context.KebabEntities
    //        .AsNoTracking()
    //        .ToListAsync();

    //    var kebabs = kebabsEntities
    //        .Select(k => Kebab.Create(k.Id, k.Name, k.Description, k.Price, k.TitleImage).Value)
    //        .ToList();

    //    return kebabs;
    //}

    public async Task<Guid> Update(Guid id, string name, string description, decimal price, string? titleImagePath = null)
    {
        var kebab = await _context.KebabEntities.FindAsync(id);

        if (kebab == null)
        {
            throw new KeyNotFoundException("Kebab not found");
        }

        kebab.Name = name;
        kebab.Description = description;
        kebab.Price = price;

        if (!string.IsNullOrEmpty(titleImagePath))
        {
            kebab.TitleImagePath = titleImagePath;
        }

        _context.KebabEntities.Update(kebab);
        await _context.SaveChangesAsync();

        return id;
    }



    //public async Task<Guid> Update(Guid id, string name, string description, decimal price)
    //{
    //    await _context.KebabEntities
    //        .Where(k => k.Id == id)
    //        .ExecuteUpdateAsync(s => s
    //            .SetProperty(k => k.Name, name)
    //            .SetProperty(k => k.Description, description)
    //            .SetProperty(k => k.Price, price));

    //    return id;
    //}
}
