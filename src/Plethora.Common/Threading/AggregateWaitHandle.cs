using System;
using System.Threading;
using Plethora.Timing;

namespace Plethora.Threading
{
    public static class WaitHandleHelper
    {
        public static readonly int MaxWaitHandles = 64;
    }

    public class AggregateWaitHandle : WaitHandle
    {
        private readonly WaitHandle[] waitHandles;

        public AggregateWaitHandle(params WaitHandle[] waitHandles)
        {
            this.waitHandles = waitHandles;
        }

        public override bool WaitOne(int millisecondsTimeout, bool exitContext)
        {
            OperationTimeout timeout = new OperationTimeout(millisecondsTimeout);

            WaitHandle[] currentHandles = null;
            for (int i = 0; i < this.waitHandles.Length; i += WaitHandleHelper.MaxWaitHandles)
            {
                int count = waitHandles.Length - i;
                count = Math.Min(count, WaitHandleHelper.MaxWaitHandles);

                if ((currentHandles == null) || (currentHandles.Length != count))
                    currentHandles = new WaitHandle[count];

                Array.Copy(waitHandles, i, currentHandles, 0, count);

                bool result = WaitHandle.WaitAll(currentHandles, timeout.Remaining);
                if (!result)
                    return false;
            }

            return true;
        }

        public override bool WaitOne(TimeSpan timeout, bool exitContext)
        {
            return this.WaitOne((int)timeout.TotalMilliseconds, exitContext);
        }
    }
}
