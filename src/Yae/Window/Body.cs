using System;
using System.Linq;
using System.Threading.Tasks;
using Yae.Templates;
using Yae.Utils;

namespace Yae.Window
{
    internal sealed class Body
    {
        private readonly int _linesPerPage;

        public Body(int linesPerPage)
        {
            _linesPerPage = linesPerPage;
        }

        public async Task RenderAsync()
        {
            await Output.WriteLineAsync(new string(Chars.Horizontal, 7) + Chars.HorizontalDown +
                                        new string(Chars.Horizontal, 142));

            var lines = Enumerable.Repeat(string.Empty, _linesPerPage);
            foreach (var line in lines)
            {
                var rowNumber = new string(Chars.Whitespace, 7) + Chars.Vertical + Chars.Whitespace;
                await Output.WriteAsync(rowNumber, ConsoleColor.Gray);
                await Output.WriteLineAsync(line);
            }

            await Output.WriteLineAsync(new string(Chars.Horizontal, 7) + Chars.HorizontalUp +
                                        new string(Chars.Horizontal, 142));
        }

        public async Task RenderLineAsync(string inputData, int lineNumber)
        {
            var rowNumber = lineNumber.ToString().PadLeft(6).PadRight(7) + Chars.Vertical + Chars.Whitespace;
            await Output.WriteAsync(rowNumber, ConsoleColor.Gray);
            await Output.WriteAsync(inputData);
            await Output.ClearLineAsync(Console.CursorLeft);
        }
    }
}