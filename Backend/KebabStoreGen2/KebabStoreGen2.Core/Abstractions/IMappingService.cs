using KebabStoreGen2.Core.Models;

namespace KebabStoreGen2.Core.Abstractions;

public interface IMappingService
{
    KebabEntity MapToKebabEntity(Kebab kebab);
}
