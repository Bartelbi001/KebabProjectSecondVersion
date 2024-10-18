using KebabStoreGen2.API.KebabStoreGen2.Core.Models;
using KebabStoreGen2.Core.Abstractions;

namespace KebabStoreGen2.Application.Services;

public class KebabsService : IKebabService
{
    private readonly IKebabsRepository _kebabsRepository;

    public KebabsService(IKebabsRepository kebabsRepository)
    {
        _kebabsRepository = kebabsRepository;
    }

    public async Task<Guid> CreateKebab(Kebab kebab)
    {
        return await _kebabsRepository.Create(kebab);
    }

    public async Task<Guid> DeleteKebab(Guid id)
    {
        return await _kebabsRepository.Delete(id);
    }

    public async Task<List<Kebab>> GetAllKebabs()
    {
        return await _kebabsRepository.GetAll();
    }

    public async Task<Kebab> GetKebabById(Guid id)
    {
        return await _kebabsRepository.Get(id);
    }

    public async Task<Guid> UpdateKebab(Guid id, string name, string description, decimal price, string? titleImagePath = null)
    {
        return await _kebabsRepository.Update(id, name, description, price, titleImagePath);
    }

    //public async Task<Guid> UpdateKebab(Guid id, string name, string description, decimal price)
    //{
    //    return await _kebabsRepository.Update(id, name, description, price);
    //}
}
