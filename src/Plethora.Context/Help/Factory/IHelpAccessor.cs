using JetBrains.Annotations;

namespace Plethora.Context.Help.Factory
{
    public interface IHelpAccessor<in TKey, out TData>
    {
        [CanBeNull]
        TData GetData([NotNull] TKey key);
    }
}
