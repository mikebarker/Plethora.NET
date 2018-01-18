using JetBrains.Annotations;

namespace Plethora.Context.Help.Factory
{
    public interface IHelpDocumentCreator<TKey, TData>
    {
        [CanBeNull]
        IHelpDocument<TData> CreateDocument([NotNull] TKey key, [NotNull] IHelpAccessor<TKey, TData> accessor);
    }


    public abstract class HelpDocumentCreatorBase<THelpDocument, TKey, TData> : IHelpDocumentCreator<TKey, TData>
        where THelpDocument : IHelpDocument<TData>
    {
        IHelpDocument<TData> IHelpDocumentCreator<TKey, TData>.CreateDocument(TKey key, IHelpAccessor<TKey, TData> accessor)
        {
            return this.CreateDocument(key, accessor);
        }

        [CanBeNull]
        public abstract THelpDocument CreateDocument([NotNull] TKey key, [NotNull] IHelpAccessor<TKey, TData> accessor);
    }
}
