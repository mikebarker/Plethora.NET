namespace Plethora.Context.Help.Factory;

public interface IHelpDocumentCreator<TKey, TData>
    where TKey : notnull
{
    IHelpDocument<TData>? CreateDocument(TKey key, IHelpAccessor<TKey, TData> accessor);
}


public abstract class HelpDocumentCreatorBase<THelpDocument, TKey, TData> : IHelpDocumentCreator<TKey, TData>
    where THelpDocument : IHelpDocument<TData>
    where TKey : notnull
{
    IHelpDocument<TData> IHelpDocumentCreator<TKey, TData>.CreateDocument(TKey key, IHelpAccessor<TKey, TData> accessor)
    {
        return this.CreateDocument(key, accessor);
    }

    public abstract THelpDocument CreateDocument(TKey key, IHelpAccessor<TKey, TData> accessor);
}
