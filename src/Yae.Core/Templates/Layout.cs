using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yae.Core.Utils;

namespace Yae.Core.Templates
{
    internal static class Layout
    {
        private static readonly Dictionary<string, string> ShortCuts = new Dictionary<string, string>
        {
            {"Esc", "Exit"},
            {"Ctrl+S", "Save"}
        };

        public static async Task RenderHeaderAsync(FileInfo file, int screenWidth)
        {
            await Output.WriteLineAsync(new string(Chars.Horizontal, screenWidth));
            await Output.WriteAsync($"{Chars.Whitespace}File:{Chars.Whitespace}", ConsoleColor.Blue);
            await Output.WriteLineAsync(file.FullName);
        }

        public static async Task RenderFooterAsync(int screenWidth)
        {
            foreach (var (key, value) in ShortCuts)
            {
                await Output.WriteAsync($"{Chars.Whitespace}[{key}]{Chars.Whitespace}", ConsoleColor.Magenta);
                await Output.WriteAsync($"{value}{Chars.Whitespace}");
            }

            await Output.WriteLineAsync(string.Empty);
            await Output.WriteLineAsync(new string(Chars.Horizontal, screenWidth));
        }

        public static async Task RenderBodyAsync(int linesPerPage, int screenWidth)
        {
            await Output.WriteLineAsync(new string(Chars.Horizontal, 7) + Chars.HorizontalDown +
                                        new string(Chars.Horizontal, screenWidth - 8));

            var lines = Enumerable.Repeat(string.Empty, linesPerPage);
            foreach (var line in lines)
            {
                var rowNumber = new string(Chars.Whitespace, 7) + Chars.Vertical + Chars.Whitespace;
                await Output.WriteAsync(rowNumber, ConsoleColor.Gray);
                await Output.WriteLineAsync(line);
            }

            await Output.WriteLineAsync(new string(Chars.Horizontal, 7) + Chars.HorizontalUp +
                                        new string(Chars.Horizontal, screenWidth - 8));
        }

        public static async Task RenderLineAsync(string inputData, int lineNumber)
        {
            var number = lineNumber == 0 ? string.Empty : lineNumber.ToString();
            var rowNumber = number.PadLeft(6).PadRight(7) + Chars.Vertical + Chars.Whitespace;
            await Output.WriteAsync(rowNumber, ConsoleColor.Gray);
            await Output.WriteAsync(inputData);
            await Output.ClearLineAsync();
        }

        public static async Task RenderEmptyLineAsync()
        {
            var rowNumber = new string(Chars.Whitespace, 7) + Chars.Vertical + Chars.Whitespace;
            await Output.WriteAsync(rowNumber, ConsoleColor.Gray);
            await Output.WriteLineAsync(string.Empty);
            await Output.ClearLineAsync();
        }
    }
}