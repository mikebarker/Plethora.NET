using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Context
{
    /// <summary>
    /// The <see cref="ActivityItemRegister"/> is a singleton instance register which
    /// can be used to track UI elements which should not cause a context change when
    /// selected.
    /// </summary>
    /// <remarks>
    /// An example of a UI items which should not cause a context change is a panel
    /// which provides more information or actions regarding the selected context.
    /// </remarks>
    /// <example>
    ///  <para>
    ///   Consider a UI similar to Visual Studio with a main centeral work-space, and
    ///   a right-hand panel which provides additional information. e.g.
    ///  </para>
    ///  <para>
    ///     +-----------------------+-------+
    ///     | Main                  |Context|
    ///     | Work-space            | Panel |
    ///     |                       |       |
    ///     |                       |       |
    ///     |                       |       |
    ///     |                       |       |
    ///     |                       |       |
    ///     +-----------------------+-------+
    ///  </para>
    ///  <para>
    ///   As the user navigates within the main work space the context panel's content
    ///   will change. At some point the user may wish to follow a hyperlink presented
    ///   within the side panel. With-out registering the context panel in the activity
    ///   register, the movement away from the main workspace will cause a ContextChanged
    ///   event and clear the panel before the user's action can be made.
    ///  </para>
    /// </example>
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
