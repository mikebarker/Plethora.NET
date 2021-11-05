using JetBrains.Annotations;
using Plethora.Collections.Sets;
using Plethora.Spacial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Plethora.Cache.Spacial
{
    public class SpacialCache<TData, TArgument> : CacheBase<TData, TArgument>
        where TArgument : IArgument<TData, TArgument>
    {
        private readonly Func<IEnumerable<TArgument>, CancellationToken, Task<IEnumerable<TData>>> getDataFromSourceAsyncCallback;

        internal SpacialCache(
            [NotNull] Func<IEnumerable<TArgument>, CancellationToken, Task<IEnumerable<TData>>> getDataFromSourceAsyncCallback)
        {
            if (getDataFromSourceAsyncCallback == null)
                throw new ArgumentNullException(nameof(getDataFromSourceAsyncCallback));


            this.getDataFromSourceAsyncCallback = getDataFromSourceAsyncCallback;
        }

        public Task<IEnumerable<TData>> GetDataAsync(
            TArgument argument,
            CancellationToken cancellationToken = default)
        {
            return this.GetDataAsync(new[] { argument }, cancellationToken);
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
}
