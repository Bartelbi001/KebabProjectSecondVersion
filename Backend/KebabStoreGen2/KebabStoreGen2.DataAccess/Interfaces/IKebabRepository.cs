using KebabStoreGen2.DataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace KebabStoreGen2.Core.Abstractions;

public interface IKebabRepository
{
    Task<Guid> Create(KebabEntity kebabEntity, IFormFile? titleImage = null, string imagePath = "");
    Task<KebabEntity> GetById(Guid id);
    Task<List<KebabEntity>> GetAll();
    Task Update(KebabEntity kebabEntity, string? titleImagePath = null, IFormFile? titleImage = null, string imagePath = "");
    Task<Guid> Delete(Guid id);
}