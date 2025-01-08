namespace KebabStoreGen2.Core.Abstractions;

public interface IMapper<TSource, TDestination>
{
    TDestination Map(TSource source);
}