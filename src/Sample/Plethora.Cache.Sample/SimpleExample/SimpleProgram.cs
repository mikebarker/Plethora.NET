using System;

namespace Plethora.Cache.Sample.SimpleExample
{
    static class SimpleProgram
    {
        public static void Run()
        {
            Console.WriteLine("Cache hits (when the data requested is in the cache) return ");
            Console.WriteLine("almost instantaneously; whilst cache misses take time for the ");
            Console.WriteLine("data source to return the data requested.");
            Console.WriteLine();
            Console.WriteLine("Press:");
            Console.WriteLine("    0-9  key of the item to be accessed");
            Console.WriteLine("    C    to clear the cache");
            Console.WriteLine("    ESC  to quite");
            Console.WriteLine();

            SimpleCache simpleCache = new SimpleCache();

            while (true)
            {
                Console.Write("> ");
                ConsoleKeyInfo x = Console.ReadKey(true);

                int numeric;
                if (int.TryParse(x.KeyChar.ToString(), out numeric))
                {
                    Console.Write("Requesting Foo Id={0}... ", numeric);
                    Foo foo = simpleCache.GetFoo(numeric);
                    Console.Write("done. ");

                    if (foo == null)
                        Console.WriteLine("- null -");
                    else
                        Console.WriteLine("Foo{{Id={0}; Value={1}}}", foo.Id, foo.Value);
                }
                else if (x.Key == ConsoleKey.C)
                {
                    simpleCache.ClearCache();
                    Console.WriteLine("Cache cleared.");
                }
                else if (x.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("Quitting...");
                    break;
                }
            }
        }
    }
}
