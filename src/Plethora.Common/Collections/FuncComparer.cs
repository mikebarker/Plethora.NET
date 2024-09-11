using System;
using System.Collections.Generic;

namespace Plethora.Collections
{
    public static class FuncComparer<TSource>
    {
        public static IComparer<TSource> Create<T1>(
            Func<TSource, T1> getValue1)
        {
            return new FuncComparer<TSource, T1>(getValue1);
        }

        public static IComparer<TSource> Create<T1, T2>(
            Func<TSource, T1> getValue1,
            Func<TSource, T2> getValue2)
        {
            return new FuncComparer<TSource, T1, T2>(getValue1, getValue2);
        }

        public static IComparer<TSource> Create<T1, T2, T3>(
            Func<TSource, T1> getValue1,
            Func<TSource, T2> getValue2,
            Func<TSource, T3> getValue3)
        {
            return new FuncComparer<TSource, T1, T2, T3>(getValue1, getValue2, getValue3);
        }

        public static IComparer<TSource> Create<T1, T2, T3, T4>(
            Func<TSource, T1> getValue1,
            Func<TSource, T2> getValue2,
            Func<TSource, T3> getValue3,
            Func<TSource, T4> getValue4)
        {
            return new FuncComparer<TSource, T1, T2, T3, T4>(getValue1, getValue2, getValue3, getValue4);
        }
    }


    public class FuncComparer<TSource, T1> : IComparer<TSource>
    {
        private readonly Func<TSource, T1> getValue1;

        public FuncComparer(
            Func<TSource, T1> getValue1)
        {
            ArgumentNullException.ThrowIfNull(getValue1);


            this.getValue1 = getValue1;
        }

        public int Compare(TSource? x, TSource? y)
        {
            if (ReferenceEquals(x, y))
                return 0;

            if (x is null)
                return 1;

            if (y is null)
                return -1;

            int result;

            result = Comparer<T1>.Default.Compare(this.getValue1(x), this.getValue1(y));
            if (result != 0)
                return result;

            return 0;
        }
    }

    public class FuncComparer<TSource, T1, T2> : IComparer<TSource>
    {
        private readonly Func<TSource, T1> getValue1;
        private readonly Func<TSource, T2> getValue2;

        public FuncComparer(
            Func<TSource, T1> getValue1,
            Func<TSource, T2> getValue2)
        {
            ArgumentNullException.ThrowIfNull(getValue1);
            ArgumentNullException.ThrowIfNull(getValue2);


            this.getValue1 = getValue1;
            this.getValue2 = getValue2;
        }

        public int Compare(TSource? x, TSource? y)
        {
            if (ReferenceEquals(x, y))
                return 0;

            if (x is null)
                return 1;

            if (y is null)
                return -1;

            int result;

            result = Comparer<T1>.Default.Compare(this.getValue1(x), this.getValue1(y));
            if (result != 0)
                return result;

            result = Comparer<T2>.Default.Compare(this.getValue2(x), this.getValue2(y));
            if (result != 0)
                return result;

            return 0;
        }
    }

    public class FuncComparer<TSource, T1, T2, T3> : IComparer<TSource>
    {
        private readonly Func<TSource, T1> getValue1;
        private readonly Func<TSource, T2> getValue2;
        private readonly Func<TSource, T3> getValue3;

        public FuncComparer(
            Func<TSource, T1> getValue1,
            Func<TSource, T2> getValue2,
            Func<TSource, T3> getValue3)
        {
            ArgumentNullException.ThrowIfNull(getValue1);
            ArgumentNullException.ThrowIfNull(getValue2);
            ArgumentNullException.ThrowIfNull(getValue3);


            this.getValue1 = getValue1;
            this.getValue2 = getValue2;
            this.getValue3 = getValue3;
        }

        public int Compare(TSource? x, TSource? y)
        {
            if (ReferenceEquals(x, y))
                return 0;

            if (x is null)
                return 1;

            if (y is null)
                return -1;

            int result;

            result = Comparer<T1>.Default.Compare(this.getValue1(x), this.getValue1(y));
            if (result != 0)
                return result;

            result = Comparer<T2>.Default.Compare(this.getValue2(x), this.getValue2(y));
            if (result != 0)
                return result;

            result = Comparer<T3>.Default.Compare(this.getValue3(x), this.getValue3(y));
            if (result != 0)
                return result;

            return 0;
        }
    }

    public class FuncComparer<TSource, T1, T2, T3, T4> : IComparer<TSource>
    {
        private readonly Func<TSource, T1> getValue1;
        private readonly Func<TSource, T2> getValue2;
        private readonly Func<TSource, T3> getValue3;
        private readonly Func<TSource, T4> getValue4;

        public FuncComparer(
            Func<TSource, T1> getValue1,
            Func<TSource, T2> getValue2,
            Func<TSource, T3> getValue3,
            Func<TSource, T4> getValue4)
        {
            ArgumentNullException.ThrowIfNull(getValue1);
            ArgumentNullException.ThrowIfNull(getValue2);
            ArgumentNullException.ThrowIfNull(getValue3);
            ArgumentNullException.ThrowIfNull(getValue4);


            this.getValue1 = getValue1;
            this.getValue2 = getValue2;
            this.getValue3 = getValue3;
            this.getValue4 = getValue4;
        }

        public int Compare(TSource? x, TSource? y)
        {
            if (ReferenceEquals(x, y))
                return 0;

            if (x is null)
                return 1;

            if (y is null)
                return -1;

            int result;

            result = Comparer<T1>.Default.Compare(this.getValue1(x), this.getValue1(y));
            if (result != 0)
                return result;

            result = Comparer<T2>.Default.Compare(this.getValue2(x), this.getValue2(y));
            if (result != 0)
                return result;

            result = Comparer<T3>.Default.Compare(this.getValue3(x), this.getValue3(y));
            if (result != 0)
                return result;

            result = Comparer<T4>.Default.Compare(this.getValue4(x), this.getValue4(y));
            if (result != 0)
                return result;

            return 0;
        }
    }
}
