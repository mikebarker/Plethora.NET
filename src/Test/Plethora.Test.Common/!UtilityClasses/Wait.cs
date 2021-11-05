using System;
using System.Diagnostics;
using System.Threading;

namespace Plethora.Test._UtilityClasses
{
    public static class Wait
    {
        public static bool For(Func<bool> prediciate, TimeSpan timeout)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            while (!prediciate())
            {
                Thread.Sleep(1);

                if (sw.Elapsed > timeout)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
