using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Yae.Core.Utils
{
    internal static class Source
    {
        public static Task<Encoding> GetEncodingAsync(FileInfo file)
        {
            using var reader = new StreamReader(file.FullName, Encoding.Default, true);
            if (reader.Peek() >= 0)
            {
                reader.Read();
            }

            return Task.FromResult(reader.CurrentEncoding);
        }

        public static Task<string[]> ReadAllLinesAsync(FileInfo file)
        {
            return File.ReadAllLinesAsync(file.FullName);
        }
    }
}