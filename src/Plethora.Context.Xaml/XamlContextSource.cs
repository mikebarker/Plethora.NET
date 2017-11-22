using System;
using System.ComponentModel;
using System.Windows;

using JetBrains.Annotations;

namespace Plethora.Context
{
    public class XamlContextSource : Freezable, IXamlContext, INotifyPropertyChanged
    {
        #region Implementation of Freezable

        /// <remarks>
        /// Required by <see cref="Freezable"/>
        /// </remarks>
        protected override Freezable CreateInstanceCore()
        {
            return new XamlContextSource();
        }

        #endregion

        #region ContextChanged Event

        /// <summary>
        /// Raised when <see cref="Context"/> property changes.
        /// </summary>
        public event EventHandler ContextChanged;

        /// <summary>
        /// Raises the <see cref="ContextChanged"/> event.
        /// </summary>
        protected virtual void OnContextChanged(EventArgs e)
        {
            var handler = this.ContextChanged;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Properties

        #region BasedOn DependencyProperty

        public static readonly DependencyProperty BasedOnProperty = DependencyProperty.Register(
            nameof(BasedOn),
            typeof(IXamlContext),
            typeof(XamlContextSource),
            new PropertyMetadata(null, BasedOnPropertyChangedCallback));

        [CanBeNull]
        public IXamlContext BasedOn
        {
            get { return (IXamlContext)this.GetValue(BasedOnProperty); }
            set { this.SetValue(BasedOnProperty, value); }
        }

        private static void BasedOnPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var contextSource = (XamlContextSource)dependencyObject;
            if (!ReferenceEquals(e.NewValue, e.OldValue))
            {
                INotifyPropertyChanged oldBase = e.OldValue as INotifyPropertyChanged;
                if (oldBase != null)
                {
                    oldBase.PropertyChanged -= contextSource.BasedOnPropertyChanged;
                }

                INotifyPropertyChanged newBase = e.NewValue as INotifyPropertyChanged;
                if (newBase != null)
                {
                    newBase.PropertyChanged += contextSource.BasedOnPropertyChanged;
                }
            }
        }

        private void BasedOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Propegate the change from the base if the property hasn't been overridden in this instance.
            switch (e.PropertyName)
            {
                case nameof(IXamlContext.ContextName):
                    if (this.ReadLocalValue(ContextNameProperty) == DependencyProperty.UnsetValue)
                        this.OnContextChanged(EventArgs.Empty);
                    break;

                case nameof(IXamlContext.Rank):
                    if (this.ReadLocalValue(RankProperty) == DependencyProperty.UnsetValue)
                        this.OnContextChanged(EventArgs.Empty);
                    break;

                case nameof(IXamlContext.Data):
                    if (this.ReadLocalValue(DataProperty) == DependencyProperty.UnsetValue)
                        this.OnContextChanged(EventArgs.Empty);
                    break;
            }
        }

        #endregion

        #region ContextName DependencyProperty

        public static readonly DependencyProperty ContextNameProperty = DependencyProperty.Register(
            nameof(ContextName),
            typeof(string),
            typeof(XamlContextSource),
            new PropertyMetadata(default(string), PropertyChangedCallback));

        [CanBeNull]
        public string ContextName
        {
            get
            {
                string contextName = (string)this.GetValue(ContextNameProperty);
                if (this.ReadLocalValue(ContextNameProperty) == DependencyProperty.UnsetValue)
                {
                    if (this.BasedOn != null)
                        contextName = this.BasedOn.ContextName;
                }
                return contextName;
            }
            set { this.SetValue(ContextNameProperty, value); }
        }

        #endregion

        #region Rank DependencyProperty

        public static readonly DependencyProperty RankProperty = DependencyProperty.Register(
            nameof(Rank),
            typeof(int),
            typeof(XamlContextSource),
            new PropertyMetadata(default(int), PropertyChangedCallback));

        public int Rank
        {
            get
            {
                int rank = (int)this.GetValue(RankProperty);
                if (this.ReadLocalValue(RankProperty) == DependencyProperty.UnsetValue)
                {
                    if (this.BasedOn != null)
                        rank = this.BasedOn.Rank;
                }
                return rank;
            }
            set { this.SetValue(RankProperty, value); }
        }

        #endregion

        #region Data DependencyProperty

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            nameof(Data),
            typeof(object),
            typeof(XamlContextSource),
            new PropertyMetadata(default(object), PropertyChangedCallback));

        [CanBeNull]
        public object Data
        {
            get
            {
                object data = this.GetValue(DataProperty);
                if (this.ReadLocalValue(DataProperty) == DependencyProperty.UnsetValue)
                {
                    if (this.BasedOn != null)
                        data = this.BasedOn.Data;
                }
                return data;
            }
            set { this.SetValue(DataProperty, value); }
        }

        #endregion

        [NotNull]
        public ContextInfo Context
        {
            get
            {
                string contextName = this.ContextName;
                if (contextName == null)
                    throw new InvalidOperationException("ContextName is null.");

                return new ContextInfo(contextName, this.Rank, this.Data);
            }
        }

        #endregion

        #region Private Methods

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var contextSource = (XamlContextSource)dependencyObject;
            if (!Equals(e.NewValue, e.OldValue))
            {
                contextSource.OnPropertyChanged(e.Property.Name);
                contextSource.OnContextChanged(EventArgs.Empty);
            }
        }

        #endregion

        #region PropertyChanged Event

        /// <summary>
        /// Raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
