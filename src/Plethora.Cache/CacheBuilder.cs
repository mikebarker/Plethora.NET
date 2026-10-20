using Plethora.Cache.Spacial;
using Plethora.Collections.Sets;
using Plethora.Spacial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Plethora.Cache;

public static class CacheBuilder
{
    public static CacheBuilder<TData, T1, TParam1, T2, T2> AddDiscreteDimension<TData, T1, TParam1, T2>(
        this CacheBuilder<TData, T1, TParam1> source,
        Func<TData, T2> getT2Func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(getT2Func);

        return new CacheBuilder<TData, T1, TParam1, T2, T2>(
            source.Dimension,
            CacheBuilder.GetDiscreteDimension(getT2Func));
    }

    public static CacheBuilder<TData, T1, TParam1, T2, Range<T2>> AddRangeDimension<TData, T1, TParam1, T2>(
        this CacheBuilder<TData, T1, TParam1> source,
        Func<TData, T2> getT2Func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(getT2Func);

        return new CacheBuilder<TData, T1, TParam1, T2, Range<T2>>(
            source.Dimension,
            CacheBuilder.GetRangeDimension(getT2Func));
    }

    public static CacheBuilder<TData, T1, TParam1, T2, TParam2, T3, T3> AddDiscreteDimension<TData, T1, TParam1, T2, TParam2, T3>(
        this CacheBuilder<TData, T1, TParam1, T2, TParam2> source,
        Func<TData, T3> getT3Func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(getT3Func);

        return new CacheBuilder<TData, T1, TParam1, T2, TParam2, T3, T3>(
            source.Dimension1,
            source.Dimension2,
            CacheBuilder.GetDiscreteDimension(getT3Func));
    }

    public static CacheBuilder<TData, T1, TParam1, T2, TParam2, T3, Range<T3>> AddRangeDimension<TData, T1, TParam1, T2, TParam2, T3>(
        this CacheBuilder<TData, T1, TParam1, T2, TParam2> source,
        Func<TData, T3> getT3Func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(getT3Func);

        return new CacheBuilder<TData, T1, TParam1, T2, TParam2, T3, Range<T3>>(
            source.Dimension1,
            source.Dimension2,
            CacheBuilder.GetRangeDimension(getT3Func));
    }

    internal static CacheBuilderDimension<TData, T, T> GetDiscreteDimension<TData, T>(
        Func<TData, T> getTFunc)
    {
        ArgumentNullException.ThrowIfNull(getTFunc);

        var dimension = new CacheBuilderDimension<TData, T, T>(
            getTFunc,
            value => new InclusiveSet<T>(value),
            set => ((InclusiveSet<T>)set).IncludedElements.First());

        return dimension;
    }

    internal static CacheBuilderDimension<TData, T, Range<T>> GetRangeDimension<TData, T>(
        Func<TData, T> getTFunc)
    {
        ArgumentNullException.ThrowIfNull(getTFunc);

        var dimension = new CacheBuilderDimension<TData, T, Range<T>>(
            getTFunc,
            range => new RangeInclusiveSet<T>(range),
            set => ((RangeInclusiveSet<T>)set).Range);

        return dimension;
    }
}

public class CacheBuilderDimension<TData, T, TParam>
{
    public CacheBuilderDimension(
        Func<TData, T> getTFunc,
        Func<TParam, ISetCore<T>> parameterFunc,
        Func<ISetCore<T>, TParam> reverseParameterFunc)
    {
        ArgumentNullException.ThrowIfNull(getTFunc);
        ArgumentNullException.ThrowIfNull(parameterFunc);
        ArgumentNullException.ThrowIfNull(reverseParameterFunc);

        this.GetTFunc = getTFunc;
        this.ParameterFunc = parameterFunc;
        this.ReverseParameterFunc = reverseParameterFunc;
    }

    public Func<TData, T> GetTFunc { get; }
    public Func<TParam, ISetCore<T>> ParameterFunc { get; }
    public Func<ISetCore<T>, TParam> ReverseParameterFunc { get; }
}

public class CacheBuilder<TData>
{
    public static CacheBuilder<TData, T, T> AddDiscreteDimension<T>(Func<TData, T> getTFunc)
    {
        ArgumentNullException.ThrowIfNull(getTFunc);

        return new CacheBuilder<TData, T, T>(
            CacheBuilder.GetDiscreteDimension(getTFunc));
    }

    public static CacheBuilder<TData, T, Range<T>> AddRangeDimension<T>(Func<TData, T> getTFunc)
    {
        ArgumentNullException.ThrowIfNull(getTFunc);

        return new CacheBuilder<TData, T, Range<T>>(
            CacheBuilder.GetRangeDimension(getTFunc));
    }
}

public class CacheBuilder<TData, T, TParam>
{
    public CacheBuilder(
        CacheBuilderDimension<TData, T, TParam> dimension)
    {
        ArgumentNullException.ThrowIfNull(dimension);

        this.Dimension = dimension;
    }

    public CacheBuilderDimension<TData, T, TParam> Dimension { get; }

    public SpacialArgument<TData, T> CreateArg(
        TParam param1)
    {
        Func<TData, Tuple<T>> getPointFromData = (d) =>
        {
            var t = this.Dimension.GetTFunc(d);
            return Tuple.Create(t);
        };

        return new SpacialArgument<TData, T>(
            getPointFromData,
            new SpaceRegion<T>(this.Dimension.ParameterFunc(param1)));
    }

    public SpacialCache<TData, SpacialArgument<TData, T>> CreateCache(
        Func<IEnumerable<TParam>, CancellationToken, Task<IEnumerable<TData>>> getDataFromSourceAsyncCallback)
    {
        ArgumentNullException.ThrowIfNull(getDataFromSourceAsyncCallback);

        Task<IEnumerable<TData>> callback(IEnumerable<SpacialArgument<TData, T>> arguments, CancellationToken cancellationToken)
        {
            var values = new List<TParam>();
            foreach (var arg in arguments)
            {
                TParam param = this.Dimension.ReverseParameterFunc(arg.Region.Dimension1);

                values.Add(param);
            }

            return getDataFromSourceAsyncCallback(values, cancellationToken);
        }

        return new SpacialCache<TData, SpacialArgument<TData, T>>(callback);
    }
}

public class CacheBuilder<TData, T1, TParam1, T2, TParam2>
{
    public CacheBuilder(
        CacheBuilderDimension<TData, T1, TParam1> dimension1,
        CacheBuilderDimension<TData, T2, TParam2> dimension2)
    {
        ArgumentNullException.ThrowIfNull(dimension1);
        ArgumentNullException.ThrowIfNull(dimension2);

        this.Dimension1 = dimension1;
        this.Dimension2 = dimension2;
    }

    public CacheBuilderDimension<TData, T1, TParam1> Dimension1 { get; }
    public CacheBuilderDimension<TData, T2, TParam2> Dimension2 { get; }

    public SpacialCache<TData, SpacialArgument<TData, T1, T2>> CreateCache(
        Func<IEnumerable<Tuple<TParam1, TParam2>>, CancellationToken, Task<IEnumerable<TData>>> getDataFromSourceAsyncCallback)
    {
        ArgumentNullException.ThrowIfNull(getDataFromSourceAsyncCallback);

        Task<IEnumerable<TData>> callback(IEnumerable<SpacialArgument<TData, T1, T2>> arguments, CancellationToken cancellationToken)
        {
            var list = new List<Tuple<TParam1, TParam2>>();
            foreach (var arg in arguments)
            {
                TParam1 param1 = this.Dimension1.ReverseParameterFunc(arg.Region.Dimension1);
                TParam2 param2 = this.Dimension2.ReverseParameterFunc(arg.Region.Dimension2);

                list.Add(Tuple.Create(param1, param2));
            }

            return getDataFromSourceAsyncCallback(list, cancellationToken);
        }

        return new SpacialCache<TData, SpacialArgument<TData, T1, T2>>(callback);
    }

    public SpacialArgument<TData, T1, T2> CreateArg(
        TParam1 param1,
        TParam2 param2)
    {
        Func<TData, Tuple<T1, T2>> getPointFromData = (d) =>
        {
            var t1 = this.Dimension1.GetTFunc(d);
            var t2 = this.Dimension2.GetTFunc(d);
            return Tuple.Create(t1, t2);
        };

        return new SpacialArgument<TData, T1, T2>(
            getPointFromData,
            new SpaceRegion<T1, T2>(
                this.Dimension1.ParameterFunc(param1),
                this.Dimension2.ParameterFunc(param2)));
    }
}

public class CacheBuilder<TData, T1, TParam1, T2, TParam2, T3, TParam3>
{
    public CacheBuilder(
        CacheBuilderDimension<TData, T1, TParam1> dimension1,
        CacheBuilderDimension<TData, T2, TParam2> dimension2,
        CacheBuilderDimension<TData, T3, TParam3> dimension3)
    {
        ArgumentNullException.ThrowIfNull(dimension1);
        ArgumentNullException.ThrowIfNull(dimension2);
        ArgumentNullException.ThrowIfNull(dimension3);

        this.Dimension1 = dimension1;
        this.Dimension2 = dimension2;
        this.Dimension3 = dimension3;
    }

    public CacheBuilderDimension<TData, T1, TParam1> Dimension1 { get; }
    public CacheBuilderDimension<TData, T2, TParam2> Dimension2 { get; }
    public CacheBuilderDimension<TData, T3, TParam3> Dimension3 { get; }

    public SpacialCache<TData, SpacialArgument<TData, T1, T2, T3>> CreateCache(
        Func<IEnumerable<Tuple<TParam1, TParam2, TParam3>>, CancellationToken, Task<IEnumerable<TData>>> getDataFromSourceAsyncCallback)
    {
        ArgumentNullException.ThrowIfNull(getDataFromSourceAsyncCallback);

        Task<IEnumerable<TData>> callback(IEnumerable<SpacialArgument<TData, T1, T2, T3>> arguments, CancellationToken cancellationToken)
        {
            var list = new List<Tuple<TParam1, TParam2, TParam3>>();
            foreach (var arg in arguments)
            {
                TParam1 param1 = this.Dimension1.ReverseParameterFunc(arg.Region.Dimension1);
                TParam2 param2 = this.Dimension2.ReverseParameterFunc(arg.Region.Dimension2);
                TParam3 param3 = this.Dimension3.ReverseParameterFunc(arg.Region.Dimension3);

                list.Add(Tuple.Create(param1, param2, param3));
            }

            return getDataFromSourceAsyncCallback(list, cancellationToken);
        }

        return new SpacialCache<TData, SpacialArgument<TData, T1, T2, T3>>(callback);
    }

    public SpacialArgument<TData, T1, T2, T3> CreateArg(
        TParam1 param1,
        TParam2 param2,
        TParam3 param3)
    {
        Func<TData, Tuple<T1, T2, T3>> getPointFromData = (d) =>
        {
            var t1 = this.Dimension1.GetTFunc(d);
            var t2 = this.Dimension2.GetTFunc(d);
            var t3 = this.Dimension3.GetTFunc(d);
            return Tuple.Create(t1, t2, t3);
        };

        return new SpacialArgument<TData, T1, T2, T3>(
            getPointFromData,
            new SpaceRegion<T1, T2, T3>(
                this.Dimension1.ParameterFunc(param1),
                this.Dimension2.ParameterFunc(param2),
                this.Dimension3.ParameterFunc(param3)));
    }

}
