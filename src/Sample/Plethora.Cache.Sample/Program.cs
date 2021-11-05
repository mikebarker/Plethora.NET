using System;
using System.Threading.Tasks;
using Plethora.Cache.Sample.BuilderExample;
using Plethora.Cache.Sample.ComplexExample;
using Plethora.Cache.Sample.SimpleExample;

namespace Plethora.Cache.Sample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            Console.WriteLine("Select which example to run:");
            Console.WriteLine("\t1. Simple demo");
            Console.WriteLine("\t2. Complex demo");
            Console.WriteLine("\t3. CacheBuilder demo");

            int result;
            bool isValid = false;
            do
            {
                ConsoleKeyInfo x = Console.ReadKey(true);

                if (int.TryParse(x.KeyChar.ToString(), out result))
                {
                    isValid =
                        (result == 1) ||
                        (result == 2) ||
                        (result == 3);
                }
            } while (!isValid);

            Console.Clear();
            switch (result)
            {
                case 1:
                    await SimpleProgram.RunAsync().ConfigureAwait(false);
                    break;
                case 2:
                    await ComplexProgram.RunAsync().ConfigureAwait(false);
                    break;
                case 3:
                    await BuilderProgram.RunAsync().ConfigureAwait(false);
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
        }
    }
}
