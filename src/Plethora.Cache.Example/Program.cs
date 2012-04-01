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

            Foo foo;
            foo = simpleCache.GetFoo(1);
            Console.WriteLine("Foo: Id={0}; Value={1}", foo.Id, foo.Value);

            foo = simpleCache.GetFoo(1);
            Console.WriteLine("Foo: Id={0}; Value={1}", foo.Id, foo.Value);

            foo = simpleCache.GetFoo(2);
            Console.WriteLine("Foo: Id={0}; Value={1}", foo.Id, foo.Value);

            foo = simpleCache.GetFoo(1);
            Console.WriteLine("Foo: Id={0}; Value={1}", foo.Id, foo.Value);

            foo = simpleCache.GetFoo(2);
            Console.WriteLine("Foo: Id={0}; Value={1}", foo.Id, foo.Value);

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
        }
    }
}
