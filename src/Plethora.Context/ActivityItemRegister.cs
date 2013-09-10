using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Context
{
    public class ActivityItemRegister
    {
        #region Singleton Implementation

        /// <summary>
        /// Singleton instance of <see cref="ActivityItemRegister"/>.
        /// </summary>
        public static readonly ActivityItemRegister Instance = new ActivityItemRegister();

        /// <summary>
        /// Initialise a new instance of the <see cref="ActivityItemRegister"/> class.
        /// </summary>
        private ActivityItemRegister()
        {
        }

        #endregion

        private readonly Dictionary<int, List<WeakReference>> activityItemDictionary =
            new Dictionary<int, List<WeakReference>>(0);

        public void RegisterActivityItem(Object item)
        {
            //Validation
            if (item == null)
                throw new ArgumentNullException("item");

            int hashCode = item.GetHashCode();

            lock (activityItemDictionary)
            {
                List<WeakReference> list;
                if (!activityItemDictionary.TryGetValue(hashCode, out list))
                {
                    list = new List<WeakReference>();
                    activityItemDictionary[hashCode] = list;
                }
                else
                {
                    //Ensure the item doesn't already exist
                    if (list.Any(weakReference => ReferenceEquals(weakReference.Target, item)))
                        return;
                }

                list.Add(new WeakReference(item));
            }
        }

        public void DeregisterActivityItem(Object item)
        {
            //Validation
            if (item == null)
                throw new ArgumentNullException("item");

            int hashCode = item.GetHashCode();
            lock (activityItemDictionary)
            {
                List<WeakReference> list;
                if (!activityItemDictionary.TryGetValue(hashCode, out list))
                    return;

                for (int i = 0; i < list.Count; i++)
                {
                    var target = list[i].Target;
                    if (target == null)
                    {
                        //Clean up the list
                        list.RemoveAt(i);
                        i--;
                    }
                    else if (Equals(item, target))
                    {
                        //Clean up the list
                        list.RemoveAt(i);
                        break;
                    }
                }

                if (list.Count == 0)
                {
                    activityItemDictionary.Remove(hashCode);
                }
            }
        }

        public bool IsActivityItem(Object item)
        {
            //Validation
            if (item == null)
                return false;

            int hashCode = item.GetHashCode();

            lock (activityItemDictionary)
            {
                List<WeakReference> list;
                if (activityItemDictionary.TryGetValue(hashCode, out list))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var target = list[i].Target;
                        if (target == null)
                        {
                            //Clean up the list
                            list.RemoveAt(i);
                            i--;
                        }
                        else if (Equals(item, target))
                        {
                            return true;
                        }
                    }

                    if (list.Count == 0)
                        activityItemDictionary.Remove(hashCode);
                }

                return false;
            }
        }
    }

}
