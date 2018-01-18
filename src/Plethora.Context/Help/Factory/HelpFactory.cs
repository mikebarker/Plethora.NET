using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Plethora.Context.Help.Factory
{
    public class HelpFactory<TKey, TData> : IHelpFactory
    {
        private readonly IHelpKeyer<TKey> keyer;
        private readonly IHelpAccessor<TKey, TData> accessor;
        private readonly IHelpDocumentCreator<TKey, TData> creator;

        public HelpFactory([NotNull] IHelpKeyer<TKey> keyer, [NotNull] IHelpAccessor<TKey, TData> accessor, [NotNull] IHelpDocumentCreator<TKey, TData> creator)
        {
            if (keyer == null)
                throw new ArgumentNullException(nameof(keyer));

            if (accessor == null)
                throw new ArgumentNullException(nameof(accessor));

            if (creator == null)
                throw new ArgumentNullException(nameof(creator));


            this.keyer = keyer;
            this.accessor = accessor;
            this.creator = creator;
        }

        [CanBeNull]
        public IEnumerable<IHelpDocument> GetHelpDocuments(IEnumerable<ContextInfo> contexts)
        {
            if (contexts == null)
                return null;

            IEnumerable<TKey> keys = contexts
                .Where(context => context != null)
                .Select(context => this.keyer.GetHelpKey(context))
                .Where(key => key != null)
                .Distinct();

            IEnumerable<IHelpDocument<TData>> documents = keys
                .Select(key => this.creator.CreateDocument(key, this.accessor))
                .Where(document => document != null);

            return documents;
        }
    }
}
