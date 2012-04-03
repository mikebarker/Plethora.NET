using System;

namespace Plethora.Cache.Example.SimpleExample
{
    static class SimpleProgram
    {
        public static void Run()
        {
            Console.Write("Cache hits (when the data requested is in the cache) return ");
            Console.Write("almost instantaneously; whilst cache misses take time for the ");
            Console.Write("data source to return the data requested.");
            Console.WriteLine();
            Console.WriteLine();


            SimpleCache simpleCache = new SimpleCache();

            Foo foo;

            Console.Write("Requesting Foo Id=1... ");
            foo = simpleCache.GetFoo(1);
            Console.WriteLine("done. Foo{{Id={0}; Value={1}}}", foo.Id, foo.Value);

            Console.Write("Requesting Foo Id=1 (again)... ");
            foo = simpleCache.GetFoo(1);
            Console.WriteLine("done. Foo{{Id={0}; Value={1}}}", foo.Id, foo.Value);

            Console.Write("Requesting Foo Id=2... ");
            foo = simpleCache.GetFoo(2);
            Console.WriteLine("done. Foo{{Id={0}; Value={1}}}", foo.Id, foo.Value);

            Console.Write("Requesting Foo Id=1 (again)... ");
            foo = simpleCache.GetFoo(1);
            Console.WriteLine("done. Foo{{Id={0}; Value={1}}}", foo.Id, foo.Value);

            Console.Write("Requesting Foo Id=2 (again)... ");
            foo = simpleCache.GetFoo(2);
            Console.WriteLine("done. Foo{{Id={0}; Value={1}}}", foo.Id, foo.Value);
            
            Console.Write("Dropping Id=2.... ");
            simpleCache.DropFooFromCache(2);
            Console.WriteLine("done.");

            Console.Write("Requesting Foo Id=1 (after dropping 2)... ");
            foo = simpleCache.GetFoo(1);
            Console.WriteLine("done. Foo{{Id={0}; Value={1}}}", foo.Id, foo.Value);

            Console.Write("Requesting Foo Id=2 (after dropping it previously)... ");
            foo = simpleCache.GetFoo(2);
            Console.WriteLine("done. Foo{{Id={0}; Value={1}}}", foo.Id, foo.Value);
        }
    }
}
