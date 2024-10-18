using KebabStoreGen2.API.KebabStoreGen2.Core.Models;

namespace KebabStoreGen2.Core.Abstractions;

public interface IKebabsRepository
{
    Task<Guid> Create(Kebab kebab);
    Task<Guid> Delete(Guid id);
    Task<List<Kebab>> GetAll();
    Task<Kebab> Get(Guid id);
    Task<Guid> Update(Guid id, string name, string description, decimal price, string? titleImagePath = null); // Обновление интерфейса
}

//public interface IKebabsRepository
//{
//    Task<Guid> Create(Kebab kebab);
//    Task<Guid> Update(Guid id, string name, string description, decimal price);
//    Task<Guid> Delete(Guid id);
//    Task<List<Kebab>> Get();
//}
