using KebabStoreGen2.API.KebabStoreGen2.Core.Models;

namespace KebabStoreGen2.Core.Abstractions;

public interface IKebabsRepository
{
    Task<Guid> Create(Kebab kebab);
    Task<Guid> Update(Guid id, string name, string description, decimal price);
    Task<Guid> Delete(Guid id);
    Task<List<Kebab>> Get();
}
