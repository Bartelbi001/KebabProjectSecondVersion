using KebabStoreGen2.Core.Contracts;

namespace KebabStoreGen2.Core.Abstractions;

public interface IKebabService
{
    Task<Guid> CreateKebab(KebabRequest kebabRequest);
    Task<KebabResponse> GetKebabById(Guid id);
    Task<List<KebabResponse>> GetAllKebabs();
    Task UpdateKebab(Guid id, KebabRequest kebabRequest);
    Task<Guid> DeleteKebab(Guid id);
}