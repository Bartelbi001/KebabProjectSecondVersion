using KebabStoreGen2.API.KebabStoreGen2.Core.Models;
using KebabStoreGen2.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KebabStoreGen2.DataAccess.Repositories;

public class KebabRepository : IKebabsRepository
{
    private readonly KebabStoreGen2DbContext _context;

    public KebabRepository(KebabStoreGen2DbContext context)
    {
        _context = context;
    }

    public Task<Guid> Create(Kebab kebab)
    {

    }

    public Task<Guid> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Kebab>> Get()
    {
        var kebabsEntities = await _context.KebabEntities
            .AsNoTracking()
            .ToListAsync();

        var kebabs = kebabsEntities
            .Select(k => Kebab.Create(k.Id, k.Name, k.Description, k.Price, ))
            .ToList();
    }

    public Task<Guid> Update(Guid id, string name, string description, decimal price)
    {
        throw new NotImplementedException();
    }
}
