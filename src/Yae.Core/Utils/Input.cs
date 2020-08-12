using System;

namespace Yae.Core.Utils
{
    internal static class Input
    {
        public static ConsoleKeyInfo ReadKey() => Console.ReadKey();

        public static string ReadLine() => Console.ReadLine();
    }
}