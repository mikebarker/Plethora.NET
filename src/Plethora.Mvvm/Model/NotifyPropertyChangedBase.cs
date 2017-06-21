using System;
using System.ComponentModel;
using System.Linq.Expressions;

using JetBrains.Annotations;

using Plethora.Linq.Expressions;

namespace Plethora.Mvvm.Model
{
    /// <summary>
    /// Base class which implements the <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Also provides the implementation logic for the <see cref="INotifyPropertyChanging"/> interface.
    /// Inheriting classes which need to implement the <see cref="INotifyPropertyChanging"/> interface should do so as:
    /// <code><![CDATA[
    ///     class MyClass : NotifyPropertyChangedBase, INotifyPropertyChanging
    ///     {
    ///         /// <summary>
    ///         /// Raised when a property value is changing.
    ///         /// </summary>
    ///         public event PropertyChangingEventHandler PropertyChanging
    ///         {
    ///             add { base.InternalPropertyChanging += value; }
    ///             remove { base.InternalPropertyChanging -= value; }
    ///         }
    ///     }
    /// ]]></code>
    /// </para>
    /// <para>
    /// The <see cref="INotifyPropertyChanging"/>  interface is not implemented directly because this might cause unwanted effects for tests such as:
    /// <code><![CDATA[
    ///     class SomeClass : NotifyPropertyChangedBase
    ///     {
    ///         // ...
    ///     }
    /// 
    ///     SomeClass someClass = new SomeClass();
    ///     bool isNotifyPropertyChanging = someClass is INotifyPropertyChanged;  //should return false if 'SomeClass' does not make use of the INotifyPropertyChanged property
    /// ]]></code>
    /// </para>
    /// </remarks>
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        private bool isNotifying = true;

        #region PropertyChanging Event

        /// <summary>
        /// Raised when a property value is changing.
        /// </summary>
        protected event PropertyChangingEventHandler InternalPropertyChanging;

        /// <summary>
        /// Raises the <see cref="InternalPropertyChanging"/> event.
        /// </summary>
        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            if (!this.IsNotifying)
                return;

            var handler = this.InternalPropertyChanging;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="InternalPropertyChanging"/> event.
        /// </summary>
        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanging([CanBeNull] string propertyName)
        {
            this.OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanging<T>([NotNull] Expression<Func<T>> propertyExpression)
        {
            string propertyName = ExpressionHelper.GetPropertyName(propertyExpression);

            this.OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
        }

        #endregion

        #region PropertyChanged Event

        /// <summary>
        /// Raised when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (!this.IsNotifying)
                return;

            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CanBeNull] string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged<T>([NotNull] Expression<Func<T>> propertyExpression)
        {
            string propertyName = ExpressionHelper.GetPropertyName(propertyExpression);

            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /// <summary>
        /// Gets and sets a flag indicating whether the <see cref="INotifyPropertyChanged.PropertyChanged"/>
        /// and <see cref="NotifyPropertyChangedImpl.InternalPropertyChanging"/> events are raised.
        /// </summary>
        /// <returns>
        /// true if the instance is raising events; otherwise false.
        /// </returns>
        public bool IsNotifying
        {
            get { return this.isNotifying; }
            set { this.isNotifying = value; }
        }
    }
}
