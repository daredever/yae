using System.IO;
using System.Threading.Tasks;
using ConsoleTextEditor;

namespace YaeTool
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            //var path = @"C:\Users\shcherbakov\RiderProjects\GitHub\yae\src\YaeTool\Program.cs";
            var path = @"C:\Users\shcherbakov\RiderProjects\GitHub\yae\src\ConsoleTextEditor\TextEditor.cs";
            var fileInfo = new FileInfo(path);
            var linesPerPage = 45;
            var textEditor = new TextEditor(fileInfo, linesPerPage);
            await textEditor.RunAsync();
        }
    }
}