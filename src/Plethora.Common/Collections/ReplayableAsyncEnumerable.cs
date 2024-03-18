using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Plethora.Collections
{
    public class ReplayableAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly IAsyncEnumerable<T> source;
        private readonly IAsyncEnumerator<T> sourceEnumerator;
        private readonly List<T> bufferedResults = new();
        private readonly SemaphoreSlim asyncLock = new(1, 1);
        private bool isEnumerationComplete = false;
        private Task<bool>? moveNextTask;

        public ReplayableAsyncEnumerable(
            IAsyncEnumerable<T> source)
        {
            this.source = source;
            this.sourceEnumerator = this.source.GetAsyncEnumerator();
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new Enumerator(this, cancellationToken);
        }

        private async Task<bool> MoveToAsync(int index)
        {
            Task<bool> moveNextTaskCopy;

            await this.asyncLock.WaitAsync();
            try
            {
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
            bool result = await this.sourceEnumerator.MoveNextAsync();

            await this.asyncLock.WaitAsync();
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
                return result;
            }
            finally
            {
                this.asyncLock.Release();
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

            public T Current => this.enumerable.bufferedResults[this.currentIndex];

            public ValueTask DisposeAsync()
            {
                return ValueTask.CompletedTask;
            }

            public async ValueTask<bool> MoveNextAsync()
            {
                this.cancellationToken.ThrowIfCancellationRequested();

                var result = await this.enumerable.MoveToAsync(++this.currentIndex);
                return result;
            }
        }
    }

}
