using System;
using System.Threading.Tasks;
using Yae.Templates;
using Yae.Utils;

namespace Yae.Window
{
    internal sealed class Footer
    {
        public async Task RenderAsync()
        {
            await Output.WriteAsync(Chars.Whitespace + "[Esc]" + Chars.Whitespace, ConsoleColor.Magenta);
            await Output.WriteLineAsync("Exit" + new string(Chars.Whitespace, 3));
            await Output.WriteLineAsync(new string(Chars.Horizontal, 150));
        }
    }
}