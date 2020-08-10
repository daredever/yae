using System;
using System.Collections.Generic;
using System.Linq;

namespace Yae.Buffer
{
    internal sealed class LinesBuffer
    {
        private readonly List<string> _lines = new List<string>();

        public int Size => _lines.Count;

        public void Write(string value, int line, int index)
        {
            var newLines = Enumerable.Repeat(string.Empty, Math.Max(line + 1 - _lines.Count, 0));
            foreach (var newLine in newLines)
            {
                _lines.Add(newLine);
            }

            _lines[line] = _lines[line].Insert(index, value);
        }

        public void NewLine(int line)
        {
            var newLines = Enumerable.Repeat(string.Empty, Math.Max(line + 1 - _lines.Count, 0));
            foreach (var newLine in newLines)
            {
                _lines.Add(newLine);
            }

            _lines.Insert(line, string.Empty);
        }

        public void Remove(int line, int index)
        {
            var newLines = Enumerable.Repeat(string.Empty, Math.Max(line + 1 - _lines.Count, 0));
            foreach (var newLine in newLines)
            {
                _lines.Add(newLine);
            }

            _lines[line] = _lines[line].Remove(index, 1);
        }

        public string GetLine(int line)
        {
            var newLines = Enumerable.Repeat(string.Empty, Math.Max(line + 1 - _lines.Count, 0));
            foreach (var newLine in newLines)
            {
                _lines.Add(newLine);
            }

            return _lines[line];
        }
    }
}