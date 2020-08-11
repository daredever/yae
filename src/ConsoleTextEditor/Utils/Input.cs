using System;

namespace ConsoleTextEditor.Utils
{
    internal static class Input
    {
        public static ConsoleKeyInfo ReadKey() => Console.ReadKey();

        public static string ReadLine() => Console.ReadLine();
    }
}