using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

using Plethora.Collections;

namespace Plethora.Mvvm.Model
{
    /// <summary>
    /// An <see cref="ObservableCollection{T}"/> which tracks the changes to it's items.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the collection's elements.
    /// </typeparam>
    public class TrackChangesObservableCollection<T> : ObservableCollection<T>, ITrackChanges
        where T : INotifyPropertyChanged, ITrackChanges
    {
        private readonly HashSet<T> added = new HashSet<T>(ReferenceEqualityComparer<T>.Default);
        private readonly HashSet<T> updated = new HashSet<T>(ReferenceEqualityComparer<T>.Default);
        private readonly HashSet<T> removed = new HashSet<T>(ReferenceEqualityComparer<T>.Default);

        private bool inBulkUpdate = false;


        /// <summary>
        /// Initialises a new instance of the <see cref="TrackChangesObservableCollection{T}"/> class.
        /// </summary>
        public TrackChangesObservableCollection()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="TrackChangesObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="items">
        /// A list of items to be initialised in the list.
        /// </param>
        public TrackChangesObservableCollection(
            IEnumerable<T> items)
            : base(items)
        {
            foreach (T item in this)
            {
                item.PropertyChanged += this.ItemPropertyChanged;
            }
        }

        protected override void ClearItems()
        {
            if (this.Count == 0)
                return;

            if (!this.inBulkUpdate)
            {
                foreach (T removedItem in this)
                {
                    if (!this.added.Contains(removedItem))
                    {
                        this.removed.Add(removedItem);
                    }

                    removedItem.PropertyChanged -= this.ItemPropertyChanged;
                }
                this.added.Clear();
                this.updated.Clear();
            }

            base.ClearItems();

            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.HasChanged)));
        }

        protected override void InsertItem(int index, T item)
        {
            bool preHasChanged = this.HasChanged;

            if (!this.inBulkUpdate)
            {
                if (this.removed.Contains(item))
                {
                    this.removed.Remove(item);
                    if (item.HasChanged)
                    {
                        this.updated.Add(item);
                    }
                }
                else
                {
                    this.added.Add(item);
                }
            }

            item.PropertyChanged += this.ItemPropertyChanged;

            base.InsertItem(index, item);

            if (preHasChanged != this.HasChanged)
            {
                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.HasChanged)));
            }
        }

        protected override void RemoveItem(int index)
        {
            bool preHasChanged = this.HasChanged;

            T item = this[index];

            if (!this.inBulkUpdate)
            {
                if (this.added.Contains(item))
                {
                    this.added.Remove(item);
                }
                else
                {
                    if (this.updated.Contains(item))
                        this.updated.Remove(item);

                    this.removed.Add(item);
                }
            }

            item.PropertyChanged -= this.ItemPropertyChanged;

            base.RemoveItem(index);

            if (preHasChanged != this.HasChanged)
            {
                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.HasChanged)));
            }
        }

        protected override void SetItem(int index, T item)
        {
            bool preHasChanged = this.HasChanged;

            T removedItem = this[index];

            if (!object.ReferenceEquals(item, removedItem))
            {
                // From RemoveItem
                if (!this.inBulkUpdate)
                {
                    if (this.added.Contains(removedItem))
                    {
                        this.added.Remove(removedItem);
                    }
                    else
                    {
                        if (this.updated.Contains(removedItem))
                            this.updated.Remove(removedItem);

                        this.removed.Add(removedItem);
                    }
                }

                removedItem.PropertyChanged -= this.ItemPropertyChanged;


                // From InsertItem
                if (!this.inBulkUpdate)
                {
                    if (this.removed.Contains(item))
                    {
                        this.removed.Remove(item);
                        if (item.HasChanged)
                        {
                            this.updated.Add(item);
                        }
                    }
                    else
                    {
                        this.added.Add(item);
                    }
                }

                item.PropertyChanged += this.ItemPropertyChanged;
            }

            base.SetItem(index, item);

            if (preHasChanged != this.HasChanged)
            {
                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.HasChanged)));
            }
        }


        private void ItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!this.inBulkUpdate)
            {
                bool preHasChanged = this.HasChanged;

                if (e is not DependentPropertyChangedEventArgs)
                {
                    if (sender is null)
                    {
                        return;
                    }

                    T item = (T)sender;

                    if (!this.added.Contains(item))
                    {
                        if (item.HasChanged)
                            this.updated.Add(item);
                        else
                            this.updated.Remove(item);
                    }
                }

                if (preHasChanged != this.HasChanged)
                {
                    this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.HasChanged)));
                }
            }
        }


        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!this.inBulkUpdate)
            {
                base.OnCollectionChanged(e);
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (!this.inBulkUpdate)
            {
                base.OnPropertyChanged(e);
            }
        }


        /// <summary>
        /// The collection of elements added to the collection.
        /// </summary>
        public ICollection<T> Added
        {
            get { return new List<T>(this.added).AsReadOnly(); }
        }

        /// <summary>
        /// The collection of elements updated in the collection.
        /// </summary>
        public ICollection<T> Updated
        {
            get { return new List<T>(this.updated).AsReadOnly(); }
        }

        /// <summary>
        /// The collection of elements removed from the collection.
        /// </summary>
        public ICollection<T> Removed
        {
            get { return new List<T>(this.removed).AsReadOnly(); }
        }

        /// <summary>
        /// Gets a flag indicating whether the instance has changed values.
        /// </summary>
        /// <returns>
        /// true if the collection has changed; otherwise false.
        /// </returns>
        public bool HasChanged
        {
            get
            {
                return
                    (this.added.Count > 0) ||
                    (this.updated.Count > 0) ||
                    (this.removed.Count > 0);
            }
        }

        /// <summary>
        /// Commit the changes in the collection.
        /// </summary>
        public void Commit()
        {
            this.inBulkUpdate = true;
            try
            {
                this.added.Clear();
                this.updated.Clear();
                this.removed.Clear();

                foreach (T item in this)
                {
                    item.Commit();
                }
            }
            finally
            {
                this.inBulkUpdate = false;
            }

            base.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.HasChanged)));
        }

        /// <summary>
        /// Rollback the changes in the instance, restoring the previous state before any changes were made.
        /// </summary>
        public void Rollback()
        {
            this.inBulkUpdate = true;
            try
            {
                for (int i = 0; i < this.Count; i++)
                {
                    T item = this[i];

                    if (this.added.Contains(item))
                    {
                        this.RemoveAt(i);
                        i--;
                    }
                    else if (this.updated.Contains(item))
                    {
                        item.Rollback();
                    }
                }

                foreach (T item in this.removed)
                {
                    item.Rollback();
                    base.Add(item);
                }

                this.added.Clear();
                this.updated.Clear();
                this.removed.Clear();
            }
            finally
            {
                this.inBulkUpdate = false;
            }

            base.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.Count)));
            base.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            base.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.HasChanged)));
        }
    }
}
