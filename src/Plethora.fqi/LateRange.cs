using System;
using System.Collections.Generic;

namespace Plethora.fqi
{
    /// <summary>
    /// Interface declaring a late-bound range.
    /// </summary>
    /// <remarks>
    /// The minimum and maximum are given by functions, allowing the value to be evaluated
    /// in a late bound fashion.
    /// </remarks>
    public interface ILateRange : ICloneable
    {
        bool HasMin { get; }
        bool HasMax { get; }

        Func<object> MinFunc { get; set; }
        Func<object> MaxFunc { get; set; }
    }

    /// <summary>
    /// A named late-bound range.
    /// </summary>
    /// <seealso cref="ILateRange"/>
    public interface INamedLateRange : ILateRange
    {
        /// <summary>
        /// The name of this late-bound range.
        /// </summary>
        string Name { get; }
    }

    public class LateRange : ILateRange
    {
        #region Fields

        private Func<object> minFunc;
        private Func<object> maxFunc;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="LateRange"/> class.
        /// </summary>
        public LateRange()
        {
            this.HasMin = false;
            this.HasMax = false;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="LateRange"/> class.
        /// </summary>
        public LateRange(Func<object> minFunc, Func<object> maxFunc)
            : this()
        {
            this.MinFunc = minFunc;
            this.MaxFunc = maxFunc;
        }
        #endregion

        #region Implementation of ILateRange

        public bool HasMin { get; private set; }

        public bool HasMax { get; private set; }

        public Func<object> MinFunc
        {
            get
            {
                if (!HasMin)
                    throw new InvalidOperationException("MinFunc not set.");

                return minFunc;
            }
            set
            {
                HasMin = true;
                this.minFunc = value;
            }
        }

        public Func<object> MaxFunc
        {
            get
            {
                if (!HasMax)
                    throw new InvalidOperationException("MaxFunc not set.");

                return maxFunc;
            }
            set
            {
                HasMax = true;
                this.maxFunc = value;
            }
        }
        #endregion

        #region Implementation of ICloneable

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public LateRange Clone()
        {
            var range = new LateRange();
            if (this.HasMax)
                range.MaxFunc = this.MaxFunc;
            if (this.HasMin)
                range.MinFunc = this.MinFunc;

            return range;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion
    }

    public class NamedLateRange : LateRange, INamedLateRange
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="NamedLateRange"/> class.
        /// </summary>
        public NamedLateRange(string name)
            : base()
        {
            this.Name = name;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="NamedLateRange"/> class.
        /// </summary>
        public NamedLateRange(string name, Func<object> minFunc, Func<object> maxFunc)
            : base(minFunc, maxFunc)
        {
            this.Name = name;
        }
        #endregion

        #region Implementation of INamedLateRange

        public string Name { get; private set; }
        #endregion

        #region Implementation of ICloneable

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public new NamedLateRange Clone()
        {
            var range = new NamedLateRange(this.Name);
            if (this.HasMax)
                range.MaxFunc = this.MaxFunc;
            if (this.HasMin)
                range.MinFunc = this.MinFunc;

            return range;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion
    }

    public static class LateRangeHelper
    {
        #region Public Methods

        public static IDictionary<string, TRange> CombineDictionariesAND<TRange>(
            IDictionary<string, TRange> x,
            IDictionary<string, TRange> y)
            where TRange : ILateRange
        {
            if ((x == null) || (x.Count == 0))
                return y;

            if ((y == null) || (y.Count == 0))
                return x;

            foreach (var yPair in y)
            {
                string member = yPair.Key;
                TRange xRestriction;
                bool result = x.TryGetValue(member, out xRestriction);
                if (!result)
                {
                    x.Add(member, yPair.Value);
                }
                else
                {
                    TRange yRestriction = yPair.Value;

                    if (xRestriction.HasMax && yRestriction.HasMax)
                    {
                        if (xRestriction.MaxFunc != yRestriction.MaxFunc)
                        {
                            var tmpFunc = xRestriction.MaxFunc; // Prevent access to modified closure
                            xRestriction.MaxFunc = () => Min(tmpFunc(), yRestriction.MaxFunc());
                        }
                    }
                    else if (yRestriction.HasMax)
                    {
                        xRestriction.MaxFunc = yRestriction.MaxFunc;
                    }

                    if (xRestriction.HasMin && yRestriction.HasMin)
                    {
                        if (xRestriction.MinFunc != yRestriction.MinFunc)
                        {
                            var tmpFunc = xRestriction.MinFunc; // Prevent access to modified closure
                            xRestriction.MinFunc = () => Min(tmpFunc(), yRestriction.MinFunc());
                        }
                    }
                    else if (yRestriction.HasMin)
                    {
                        xRestriction.MinFunc = yRestriction.MinFunc;
                    }
                }
            }
            return x;
        }

        public static IDictionary<string, TRange> CombineDictionariesOR<TRange>(
            IDictionary<string, TRange> x,
            IDictionary<string, TRange> y)
            where TRange : ILateRange
        {
            if (x == null)
                return null;

            if (y == null)
                return null;

            var rtn = new Dictionary<string, TRange>();
            foreach (var pair in y)
            {
                string name = pair.Key;
                TRange xRestriction;
                bool result = x.TryGetValue(name, out xRestriction);
                if (result)
                {
                    TRange yRestriction = pair.Value;
                    var restriction = (TRange)xRestriction.Clone();

                    if (xRestriction.HasMax && yRestriction.HasMax)
                    {
                        if (xRestriction.MaxFunc != yRestriction.MaxFunc)
                        {
                            var tmpFunc = xRestriction.MaxFunc; // Prevent access to modified closure
                            restriction.MaxFunc = () => Max(tmpFunc(), yRestriction.MaxFunc());
                        }
                    }
                    else if (xRestriction.HasMax)
                    {
                        restriction.MaxFunc = xRestriction.MaxFunc;
                    }
                    else if (yRestriction.HasMax)
                    {
                        restriction.MaxFunc = yRestriction.MaxFunc;
                    }

                    if (xRestriction.HasMin && yRestriction.HasMin)
                    {
                        if (xRestriction.MinFunc != yRestriction.MinFunc)
                        {
                            var tmpFunc = xRestriction.MinFunc; // Prevent access to modified closure
                            restriction.MinFunc = () => Min(tmpFunc(), yRestriction.MinFunc());
                        }
                    }
                    else if (xRestriction.HasMin)
                    {
                        restriction.MinFunc = xRestriction.MinFunc;
                    }
                    else if (yRestriction.HasMin)
                    {
                        restriction.MinFunc = yRestriction.MinFunc;
                    }

                    rtn.Add(name, restriction);
                }
            }
            return rtn;
        }
        #endregion

        #region Private Methods

        private static T Min<T>(T a, T b)
        {
            int result = Comparer<T>.Default.Compare(a, b);
            if (result < 0)       // a < b
                return a;
            else if (result == 0) // a == b
                return a;
            else                  // a > b
                return b;
        }

        private static T Max<T>(T a, T b)
        {
            int result = Comparer<T>.Default.Compare(a, b);
            if (result < 0)       // a < b
                return b;
            else if (result == 0) // a == b
                return a;
            else                  // a > b
                return a;
        }
        #endregion
    }
}
