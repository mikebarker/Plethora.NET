using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Plethora.Mvvm
{
    /// <summary>
    /// Base class which implements the <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Also provides the implementation logic for the <see cref="INotifyPropertyChanging"/> interface.
    /// Inheriting classes which need to implement the <see cref="INotifyPropertyChanging"/> interface should do so as:
    /// <code><![CDATA[
    ///     class MyClass : NotifyPropertyChanged, INotifyPropertyChanging
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
    ///     class SomeClass : NotifyPropertyChanged
    ///     {
    ///         // ...
    ///     }
    /// 
    ///     SomeClass someClass = new SomeClass();
    ///     bool isNotifyPropertyChanging = someClass is INotifyPropertyChanging;  //should return false if 'SomeClass' does not implement the INotifyPropertyChanging interface
    /// ]]></code>
    /// </para>
    /// </remarks>
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        private bool isNotifying = true;

        protected bool ChangeProperty<T>(T value, ref T field, [CallerMemberName] string propertyName = null)
        {
            if (Equals(value, field))
                return false;

            this.OnPropertyChanging(propertyName);
            field = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

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
        protected void OnPropertyChanging([CanBeNull][CallerMemberName] string propertyName = null)
        {
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
        protected void OnPropertyChanged([CanBeNull][CallerMemberName] string propertyName = null)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /// <summary>
        /// Gets and sets a flag indicating whether the <see cref="INotifyPropertyChanged.PropertyChanged"/>
        /// and <see cref="NotifyPropertyChanged.InternalPropertyChanging"/> events are raised.
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
