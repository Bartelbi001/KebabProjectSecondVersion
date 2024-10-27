﻿using KebabStoreGen2.Core.Abstractions;
using KebabStoreGen2.Core.Models;

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

    public async Task<Guid> UpdateKebab(Guid id, string kebabName, string kebabDescription, decimal price, string? titleImagePath = null)
    {
        return await _kebabsRepository.Update(id, kebabName, kebabDescription, price, titleImagePath);
    }
}