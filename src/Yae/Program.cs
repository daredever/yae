using System.IO;
using System.Threading.Tasks;
using Yae.Window;

namespace Yae
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            var path = @"C:\Users\shcherbakov\RiderProjects\GitHub\yae\src\Yae\Program.cs";
            var fileInfo = new FileInfo(path);
            var linesPerPage = 20;
            var textEditor = new TextEditor(fileInfo, linesPerPage);
            await textEditor.RunAsync();
        }
    }
}