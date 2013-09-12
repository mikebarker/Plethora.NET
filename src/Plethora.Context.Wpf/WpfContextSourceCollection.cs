using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Plethora.Context.Wpf
{
    /// <summary>
    /// A collection of context sources.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   Elements added to this class must implement the <see cref="IWpfContextSource"/> interface.
    ///  </para>
    ///  <para>
    ///   This class inherits from <see cref="FreezableCollection{T}"/> to allow the DataContext
    ///   to flow through the context source tree.
    ///  </para>
    /// </remarks>
    public class WpfContextSourceCollection : FreezableCollection<Freezable>, IWpfContextSource
    {
        public WpfContextSourceCollection()
        {
            ((INotifyCollectionChanged)this).CollectionChanged += this_CollectionChanged;
        }


        #region Implementation Of IWpfContextSource

        #region ContextChanged Event

        /// <summary>
        /// Raised when <see cref="Contexts"/> property changes.
        /// </summary>
        public event EventHandler ContextChanged;

        /// <summary>
        /// Raises the <see cref="ContextChanged"/> event.
        /// </summary>
        private void OnContextChanged()
        {
            OnContextChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="ContextChanged"/> event.
        /// </summary>
        protected virtual void OnContextChanged(EventArgs e)
        {
            var handler = ContextChanged;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        public IEnumerable<ContextInfo> Contexts
        {
            get
            {
                var contexts = this
                    .SelectMany(item => ((IWpfContextSource)item).Contexts)
                    .ToArray();

                return contexts;
            }
        }

        #endregion


        private UIElement uiElement;

        public UIElement UIElement
        {
            get { return uiElement; }
            set
            {
                if (uiElement != null)
                    DeregisterElement(uiElement);

                uiElement = value;

                if (uiElement != null)
                    RegisterUIElement(uiElement);
            }
        }

        private void RegisterUIElement(UIElement element)
        {
            //Do not hook up the event model in design mode
            if (DesignerProperties.GetIsInDesignMode(element))
                return;

            foreach (var wpfContextSource in this)
            {
                ((IWpfContextSource)wpfContextSource).UIElement = element;
            }
        }

        private void DeregisterElement(UIElement element)
        {
            //Do not hook up the event model in design mode
            if (DesignerProperties.GetIsInDesignMode(element))
                return;

            foreach (var wpfContextSource in this)
            {
                ((IWpfContextSource)wpfContextSource).UIElement = null;
            }
        }


        void contextSource_ContextChanged(object sender, EventArgs e)
        {
            this.OnContextChanged();
        }


        //A full list of previous elements is required to support Reset actions, so that old elements can be unloaded correctly.
        List<Freezable> prevList = new List<Freezable>(); 

        private void this_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    //Do nothing. Order is not important
                    break;

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:

                    bool isReset = (e.Action != NotifyCollectionChangedAction.Reset);

                    var oldItems = (!isReset) ? e.OldItems : prevList;
                    var newItems = (!isReset) ? e.NewItems : this;


                    if (oldItems != null)
                    {
                        foreach (var oldItem in oldItems)
                        {
                            var wpfContextSource = (IWpfContextSource)oldItem;
                            wpfContextSource.ContextChanged -= contextSource_ContextChanged;
                            wpfContextSource.UIElement = null;

                            if (!isReset)
                                prevList.Remove((Freezable)oldItem);
                        }
                    }

                    if (newItems != null)
                    {
                        foreach (var newItem in newItems)
                        {
                            if (!(newItem is IWpfContextSource))
                                throw new ArgumentException(string.Format("Items must be of type {0}.",
                                                                          typeof (IWpfContextSource).Name));

                            var wpfContextSource = (IWpfContextSource)newItem;
                            wpfContextSource.UIElement = this.UIElement;
                            wpfContextSource.ContextChanged += contextSource_ContextChanged;

                            if (!isReset)
                                prevList.Add((Freezable)newItem);
                        }
                    }

                    if (isReset)
                        prevList = new List<Freezable>(this);
                    break;

                default:
                    throw new InvalidOperationException(string.Format("Unsupproted action {0}", e.Action));
            }
        }


    }
}
