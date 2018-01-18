using System;

using JetBrains.Annotations;

using Plethora.Context.Help.Factory;

namespace Plethora.Context.Help.HelpDocuments
{
    /// <summary>
    /// An creator for <see cref="RtfHelpDocument"/>.
    /// </summary>
    public class RtfHelpDocumentCreator<TKey> : HelpDocumentCreatorBase<RtfHelpDocument, TKey, string>
    {
        public override RtfHelpDocument CreateDocument(TKey key, IHelpAccessor<TKey, string> accessor)
        {
            return new RtfHelpDocument<TKey>(key, accessor);
        }
    }

    /// <summary>
    /// An implementation of <see cref="IHelpDocument"/> representing RTF documents.
    /// </summary>
    public abstract class RtfHelpDocument : IHelpDocument<string>
    {
        private bool dataReceived = false;
        private string data;

        object IHelpDocument.Data
        {
            get { return this.Data; }
        }

        [CanBeNull]
        public string Data
        {
            get
            {
                if (!this.dataReceived)
                {
                    this.data = this.GetData();
                    this.dataReceived = true;
                }
                return this.data;
            }
        }

        [CanBeNull]
        protected abstract string GetData();
    }

    internal class RtfHelpDocument<TKey> : RtfHelpDocument
    {
        private readonly TKey key;
        private readonly IHelpAccessor<TKey, string> accessor;

        public RtfHelpDocument([NotNull] TKey key, [NotNull] IHelpAccessor<TKey, string> accessor)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (accessor == null)
                throw new ArgumentNullException(nameof(accessor));


            this.key = key;
            this.accessor = accessor;
        }

        [CanBeNull]
        protected override string GetData()
        {
            return this.accessor.GetData(this.key);
        }
    }
}
