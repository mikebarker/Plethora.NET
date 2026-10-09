using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Plethora.Collections;

/// <summary>
/// Provides a replayable view over a shared asynchronous source.
/// </summary>
/// <remarks>
/// Dispose this instance when abandoning the source before it is fully enumerated.
/// Disposing an individual enumerator does not dispose the shared source.
/// </remarks>
public class ReplayableAsyncEnumerable<T> : IAsyncEnumerable<T>, IAsyncDisposable
{
    private readonly IAsyncEnumerable<T> source;
    private readonly IAsyncEnumerator<T> sourceEnumerator;
    private readonly List<T> bufferedResults = new();
    private readonly SemaphoreSlim asyncLock = new(1, 1);
    private readonly SemaphoreSlim sourceLock = new(1, 1);
    private bool isEnumerationComplete = false;
    private bool isDisposed = false;
    private bool isSourceEnumeratorDisposed = false;
    private Task<bool>? moveNextTask;

    public ReplayableAsyncEnumerable(
        IAsyncEnumerable<T> source)
    {
        this.source = source;
        this.sourceEnumerator = this.source.GetAsyncEnumerator();
    }

    /// <inheritdoc/>
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new Enumerator(this, cancellationToken);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        Task<bool>? moveNextTaskCopy;

        await this.asyncLock.WaitAsync().ConfigureAwait(false);
        try
        {
            this.isDisposed = true;
            moveNextTaskCopy = this.moveNextTask;
        }
        finally
        {
            this.asyncLock.Release();
        }

        try
        {
            if (moveNextTaskCopy is not null)
                await moveNextTaskCopy.ConfigureAwait(false);
        }
        finally
        {
            await this.DisposeSourceEnumeratorAsync().ConfigureAwait(false);
        }
    }

    private async Task<bool> MoveToAsync(int index)
    {
        Task<bool> moveNextTaskCopy;

        await this.asyncLock.WaitAsync();
        try
        {
            ObjectDisposedException.ThrowIf(this.isDisposed, this);

            if (index < this.bufferedResults.Count)
            {
                return true;
            }
            else if (index == this.bufferedResults.Count)
            {
                if (this.isEnumerationComplete)
                {
                    return false;
                }
                else
                {
                    if (this.moveNextTask is null)
                    {
                        this.moveNextTask = this.MoveNextAsync();
                    }

                    moveNextTaskCopy = this.moveNextTask;
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        finally
        {
            this.asyncLock.Release();
        }

        // Await completion outside the lock
        return await moveNextTaskCopy;
    }

    private async Task<bool> MoveNextAsync()
    {
        bool result;
        try
        {
            await this.sourceLock.WaitAsync().ConfigureAwait(false);
            try
            {
                if (this.isSourceEnumeratorDisposed)
                    throw new ObjectDisposedException(nameof(ReplayableAsyncEnumerable<T>));

                result = await this.sourceEnumerator.MoveNextAsync().ConfigureAwait(false);
            }
            finally
            {
                this.sourceLock.Release();
            }
        }
        catch
        {
            await this.DisposeSourceEnumeratorAsync().ConfigureAwait(false);
            throw;
        }

        await this.asyncLock.WaitAsync().ConfigureAwait(false);
        try
        {
            if (result)
            {
                this.bufferedResults.Add(this.sourceEnumerator.Current);
            }
            else
            {
                this.isEnumerationComplete = true;
            }

            this.moveNextTask = null;
        }
        finally
        {
            this.asyncLock.Release();
        }

        if (!result)
            await this.DisposeSourceEnumeratorAsync().ConfigureAwait(false);

        return result;
    }

    private async ValueTask DisposeSourceEnumeratorAsync()
    {
        await this.sourceLock.WaitAsync().ConfigureAwait(false);
        try
        {
            if (this.isSourceEnumeratorDisposed)
                return;

            this.isSourceEnumeratorDisposed = true;
            await this.sourceEnumerator.DisposeAsync().ConfigureAwait(false);
        }
        finally
        {
            this.sourceLock.Release();
        }
    }

    private class Enumerator : IAsyncEnumerator<T>
    {
        private readonly ReplayableAsyncEnumerable<T> enumerable;
        private readonly CancellationToken cancellationToken;
        private int currentIndex;

        public Enumerator(
            ReplayableAsyncEnumerable<T> enumerable,
            CancellationToken cancellationToken)
        {
            this.enumerable = enumerable;
            this.cancellationToken = cancellationToken;

            this.currentIndex = -1;
        }

        /// <inheritdoc/>
        public T Current => this.enumerable.bufferedResults[this.currentIndex];

        /// <inheritdoc/>
        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }

        /// <inheritdoc/>
        public async ValueTask<bool> MoveNextAsync()
        {
            this.cancellationToken.ThrowIfCancellationRequested();

            var result = await this.enumerable.MoveToAsync(++this.currentIndex);
            return result;
        }
    }
}
