using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Yae.Core;

namespace Yae.Tool
{
    public sealed class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.HelpOption("-h|--help");
            app.VersionOption("-v|--version", "0.0.1");
            
            var optionFile = app.Option("-f|--file <FILE>", "File", CommandOptionType.SingleValue);
            var optionLinesPerPage = app.Option<int>("-n|--count <N>", "Lines per page", CommandOptionType.SingleValue);

            app.OnExecuteAsync(async cancellationToken =>
            {
                var path = optionFile.HasValue() ? optionFile.Value() : string.Empty;
                var linesPerPage = optionLinesPerPage.HasValue() ? optionLinesPerPage.ParsedValue : 30;
                var fileInfo = new FileInfo(path);
                var textEditor = new TextEditor(fileInfo, linesPerPage);
                await textEditor.RunAsync();
            });

            return app.Execute(args);
        }
    }
}