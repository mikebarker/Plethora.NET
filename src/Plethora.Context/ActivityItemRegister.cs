using System;
using System.Collections.Generic;

namespace Plethora.Context
{
    public class ActivityItemRegister
    {
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
