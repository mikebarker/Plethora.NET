using System;
using System.Collections.Specialized;
using System.Windows;

namespace Plethora.Context.Wpf
{
    /// <summary>
    /// A collection of context sources templates.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   Elements added to this class must implement the <see cref="IWpfContextSource"/> interface.
    ///  </para>
    ///  <para>
    ///   This class inherits from <see cref="FreezableCollection{T}"/> to allow the WPF DataContext
    ///   to flow through the context source tree.
    ///  </para>
    /// </remarks>
    /// <example>See the example presented in <see cref="WpfContextSourceTemplate"/>.</example>
    /// <seealso cref="WpfContextSourceTemplate"/>
    public class WpfContextSourceTemplateCollection : FreezableCollection<Freezable>, IWpfContextSourceTemplate
    {
        public WpfContextSourceTemplateCollection()
        {
            ((INotifyCollectionChanged)this).CollectionChanged += CollectionChanged;
        }

        void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Freezable item in e.NewItems)
                    {
                        if (!(item is IWpfContextSourceTemplate))
                            throw new ArgumentException();
                    }
                    break;
            }
        }

        WpfContextSourceBase IWpfContextSourceTemplate.CreateContent()
        {
            var collection = new WpfContextSourceCollection();

            foreach (Freezable freezable in this)
            {
                var content = ((IWpfContextSourceTemplate)freezable).CreateContent();
                collection.Add(content);
            }

            return collection;
        }
    }
}
