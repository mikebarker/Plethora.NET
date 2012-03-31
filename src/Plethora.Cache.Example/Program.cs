using System;

namespace Plethora.Cache.Example
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SimpleCache simpleCache = new SimpleCache();

            var data0 = simpleCache.GetFoo(1);
            var data1 = simpleCache.GetFoo(1);
            var data2 = simpleCache.GetFoo(2);

            int a = 0;
        }
    }
}
