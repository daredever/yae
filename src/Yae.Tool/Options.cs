using CommandLine;

namespace Yae.Tool
{
    internal sealed class Options
    {
        [Option('f', "file",
            Required = true,
            HelpText = "Set existing file to open.")]
        public string File { get; set; }

        [Option('n', "count",
            Required = false,
            HelpText = "Set lines per page count in range [1, 100]",
            Default = 30)]
        public int LinesCount { get; set; }
    }
}