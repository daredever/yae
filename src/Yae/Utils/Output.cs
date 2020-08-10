using System;
using System.Threading.Tasks;
using Yae.Templates;

namespace Yae.Utils
{
    // todo thread safe
    internal static class Output
    {
        public static Task WriteLineAsync(string value)
        {
            return Console.Out.WriteLineAsync(value);
        }

        public static Task WriteAsync(string value)
        {
            return Console.Out.WriteAsync(value);
        }

        public static async Task WriteAsync(string value, ConsoleColor foreground)
        {
            var previousForeground = Console.ForegroundColor;
            Console.ForegroundColor = foreground;
            await Console.Out.WriteAsync(value);
            Console.ForegroundColor = previousForeground;
        }

        public static async Task WriteAsync(string value, ConsoleColor foreground, ConsoleColor background)
        {
            var previousForeground = Console.ForegroundColor;
            var previousBackground = Console.BackgroundColor;
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            await Console.Out.WriteAsync(value);
            Console.ForegroundColor = previousForeground;
            Console.BackgroundColor = previousBackground;
        }

        public static void MoveCursor(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        public static void HideCursor()
        {
            Console.CursorVisible = false;
        }

        public static void ShowCursor()
        {
            Console.CursorVisible = true;
        }

        public static void ClearScreen()
        {
            Console.Clear();
        }

        public static async Task ClearLineAsync(int startIndex = 0, int length = -1)
        {
            if (length < 0)
            {
                length = Console.BufferWidth - startIndex - 1;
            }

            Console.CursorLeft = startIndex;
            await Console.Out.WriteAsync(new string(Chars.Whitespace, length));
            Console.CursorLeft = startIndex;
        }
    }
}