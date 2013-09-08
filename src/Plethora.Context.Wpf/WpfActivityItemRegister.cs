using System.Collections.Specialized;
using System.Windows;

namespace Plethora.Context.Wpf
{
    public class WpfActivityItemRegister : FreezableCollection<DependencyObject>
    {
        private readonly ActivityItemRegister innerActivityItemRegister = new ActivityItemRegister();

        public WpfActivityItemRegister()
        {
            ((INotifyCollectionChanged)this).CollectionChanged += this_CollectionChanged;
        }

        void this_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                    {
                        innerActivityItemRegister.RegisterActivityItem(newItem);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in e.OldItems)
                    {
                        innerActivityItemRegister.RegisterActivityItem(oldItem);
                    }
                    break;
            }
        }

        public ActivityItemRegister ActivityItemRegister
        {
            get { return this.innerActivityItemRegister; }
        }
    }
}
