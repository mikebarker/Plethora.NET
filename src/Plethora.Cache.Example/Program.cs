using System;
using Plethora.Cache.Example.ComplexExample;
using Plethora.Cache.Example.SimpleExample;

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
            Console.WriteLine("Select which example to run:");
            Console.WriteLine("\t1. Simple demo");
            Console.WriteLine("\t2. Comnplex demo");

            int result;
            bool isValid = false;
            do
            {
                ConsoleKeyInfo x = Console.ReadKey(true);

                if (int.TryParse(x.KeyChar.ToString(), out result))
                {
                    isValid =
                        (result == 1) ||
                        (result == 2);
                }
            } while (!isValid);

            Console.Clear();
            switch (result)
            {
                case 1:
                    SimpleProgram.Run();
                    break;
                case 2:
                    ComplexProgram.Run();
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
        }
    }
}
