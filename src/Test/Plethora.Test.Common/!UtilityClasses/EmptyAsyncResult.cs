using System;
using System.Threading;

namespace Plethora.Test.UtilityClasses
{
    class EmptyAsyncResult : IAsyncResult
    {
        #region Fields

        private readonly WaitHandle asyncWaitHandle = new ManualResetEvent(true);

        #endregion

        #region Implementation of IAsyncResult

        public bool IsCompleted
        {
            get { return true; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return asyncWaitHandle; }
        }

        public object AsyncState
        {
            get { return null; }
        }

        public bool CompletedSynchronously
        {
            get { return true; }
        }

        #endregion
    }
}
