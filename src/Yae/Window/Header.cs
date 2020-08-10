using System;
using System.IO;
using System.Threading.Tasks;
using Yae.Templates;
using Yae.Utils;

namespace Yae.Window
{
    internal sealed class Header
    {
        private readonly FileInfo _file;

        public Header(FileInfo file)
        {
            _file = file;
        }

        public async Task RenderAsync()
        {
            await Output.WriteLineAsync(new string(Chars.Horizontal, 150));
            await Output.WriteAsync(Chars.Whitespace + "File:" + Chars.Whitespace, ConsoleColor.Blue);
            await Output.WriteLineAsync(_file.FullName);
        }
    }
}