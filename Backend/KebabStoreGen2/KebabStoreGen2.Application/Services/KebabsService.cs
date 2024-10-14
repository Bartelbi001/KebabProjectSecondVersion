using KebabStoreGen2.API.KebabStoreGen2.Core.Models;
using KebabStoreGen2.Core.Abstractions;

namespace KebabStoreGen2.Application.Services;

public class KebabsService : IKebabService
{
    public Task<Guid> CreateKebab(Kebab kebab)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> DeleteKebab(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Kebab>> GetAllKebabs()
    {
        throw new NotImplementedException();
    }

    public Task<Kebab> UpdateKebab(Guid id, string name, string description, decimal price)
    {
        throw new NotImplementedException();
    }
}
