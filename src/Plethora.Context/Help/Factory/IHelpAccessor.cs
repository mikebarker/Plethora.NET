namespace Plethora.Context.Help.Factory;

public interface IHelpAccessor<in TKey, out TData>
    where TKey : notnull
{
    TData GetData(TKey key);
}
