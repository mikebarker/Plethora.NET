using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Plethora.Cache.Spacial;

public class SpacialCache<TData, TArgument> : CacheBase<TData, TArgument>
    where TArgument : IArgument<TData, TArgument>
{
    private readonly Func<IEnumerable<TArgument>, CancellationToken, Task<IEnumerable<TData>>> getDataFromSourceAsyncCallback;

    internal SpacialCache(
        Func<IEnumerable<TArgument>, CancellationToken, Task<IEnumerable<TData>>> getDataFromSourceAsyncCallback)
    {
        ArgumentNullException.ThrowIfNull(getDataFromSourceAsyncCallback);

        this.getDataFromSourceAsyncCallback = getDataFromSourceAsyncCallback;
    }

    public Task<IEnumerable<TData>> GetDataAsync(
        TArgument argument,
        CancellationToken cancellationToken = default)
    {
        return this.GetDataAsync([argument], cancellationToken);
    }

    public new Task<IEnumerable<TData>> GetDataAsync(
        IEnumerable<TArgument> arguments,
        CancellationToken cancellationToken = default)
    {
        return base.GetDataAsync(arguments, cancellationToken);
    }

    protected override Task<IEnumerable<TData>> GetDataFromSourceAsync(
        IEnumerable<TArgument> arguments,
        CancellationToken cancellationToken)
    {
        return getDataFromSourceAsyncCallback(arguments, cancellationToken);
    }
}
