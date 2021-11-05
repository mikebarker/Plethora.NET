using System;
using System.Threading.Tasks;

namespace Plethora.Cache.Sample.SimpleExample
{
    static class SimpleProgram
    {
        public static async Task RunAsync()
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
                    Console.Write("Requesting Person Id={0}... ", numeric);
                    Person person = await simpleCache.GetPersonAsync(numeric).ConfigureAwait(false);
                    Console.Write("done. ");

                    if (person == null)
                        Console.WriteLine("- null -");
                    else
                        Console.WriteLine($"Person[Id={person.Id}; Name={person.Name}]");
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
