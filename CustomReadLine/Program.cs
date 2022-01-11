using System;
using System.Text;

namespace CustomReadLine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Cancellable ReadLine");
            Console.WriteLine("====================\n");

            Console.WriteLine("Calling at start of screen");
            var value1 = XConsole.CancellableReadLine(out var isEsc1);

            Console.WriteLine("\nCalling in middle of text");
            Console.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");

            var value2 = XConsole.CancellableReadLine(out var isEsc2);

            if (!isEsc1 && !isEsc2)
            {
                Console.WriteLine("\nThis is what you wrote using CancellableReadLine()");
                Console.WriteLine("Call from start of screen");
                Console.WriteLine($"'{value1}'");
                Console.WriteLine("\nCall from middle of text");
                Console.WriteLine($"'{value2}'");

                Console.WriteLine("Normal ReadLine");
                Console.ReadLine();
                Console.WriteLine("\nPress any key to exit");
                Console.ReadKey();
            }
        }
    }
}
