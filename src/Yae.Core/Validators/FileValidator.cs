using System;
using System.Collections.Generic;
using System.IO;

namespace Yae.Core.Validators
{
    internal static class FileValidator
    {
        private static readonly List<string> SupportedExtensions = new List<string>
        {
            ".cs",
            ".csproj",
            ".sln",
            ".xml",
            ".json",
            ".md",
        };

        public static void Validate(FileInfo file)
        {
            if (!SupportedExtensions.Contains(file.Extension))
            {
                throw new ArgumentException($"Unsupported file extension '{file.Extension}'");
            }

            if (!file.Exists)
            {
                throw new FileNotFoundException();
            }
        }
    }
}