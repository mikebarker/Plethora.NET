using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Context.Help.Factory;

public class HelpFactory<TKey, TData> : IHelpFactory
    where TKey : notnull
{
    private readonly IHelpKeyer<TKey> keyer;
    private readonly IHelpAccessor<TKey, TData> accessor;
    private readonly IHelpDocumentCreator<TKey, TData> creator;

    public HelpFactory(IHelpKeyer<TKey> keyer, IHelpAccessor<TKey, TData> accessor, IHelpDocumentCreator<TKey, TData> creator)
    {
        ArgumentNullException.ThrowIfNull(keyer);
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(creator);

        this.keyer = keyer;
        this.accessor = accessor;
        this.creator = creator;
    }

    public IEnumerable<IHelpDocument>? GetHelpDocuments(IEnumerable<ContextInfo?>? contexts)
    {
        if (contexts == null)
            return null;

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        IEnumerable<TKey> keys = contexts
            .Where(context => context is not null)
            .Select(context => this.keyer.GetHelpKey(context!))
            .Where(key => key != null)
            .Distinct();

        IEnumerable<IHelpDocument<TData>> documents = keys
            .Select(key => this.creator.CreateDocument(key, this.accessor))
            .Where(document => document != null);

        return documents;
#pragma warning restore CS8619
    }
}
