﻿using KebabStoreGen2.API.KebabStoreGen2.Core.Models;

namespace KebabStoreGen2.Core.Abstractions;

public interface IKebabService
{
    Task<Guid> CreateKebab(Kebab kebab);
    Task<Guid> DeleteKebab(Guid id);
    Task<List<Kebab>> GetAllKebabs();
    Task<Kebab> UpdateKebab(Guid id, string name, string description, decimal price);
}