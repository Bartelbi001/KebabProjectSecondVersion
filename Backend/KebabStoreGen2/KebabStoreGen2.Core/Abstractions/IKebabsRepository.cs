using KebabStoreGen2.Core.Models;

namespace KebabStoreGen2.Core.Abstractions;

public interface IKebabsRepository
{
    Task<Guid> Create(Kebab kebab);
    Task<Guid> Delete(Guid id);
    Task<List<Kebab>> GetAll();
    Task<Kebab> Get(Guid id);
    Task<Guid> Update(Guid id, string kebabName, string kebabDescription, decimal price, string? titleImagePath = null);
}
