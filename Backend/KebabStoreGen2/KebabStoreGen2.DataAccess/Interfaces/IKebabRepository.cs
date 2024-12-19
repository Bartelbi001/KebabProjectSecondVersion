using KebabStoreGen2.DataAccess.Entities;

namespace KebabStoreGen2.Core.Abstractions;

public interface IKebabRepository
{
    Task<Guid> Create(KebabEntity kebabEntity);
    Task<KebabEntity> GetById(Guid id);
    Task<List<KebabEntity>> GetAll();
    Task Update(KebabEntity kebabEntity);
    Task<Guid> Delete(Guid id);
}