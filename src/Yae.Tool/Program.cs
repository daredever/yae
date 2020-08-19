using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Yae.Core;

namespace Yae.Tool
{
    public sealed class Program
    {
        private static async Task Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            await result.WithParsedAsync(options => RunEditorAsync(options));
        }

        private static async Task RunEditorAsync(Options options)
        {
            try
            {
                var file = new FileInfo(options.File);
                var textEditor = new TextEditor(file, options.LinesCount);
                await textEditor.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}