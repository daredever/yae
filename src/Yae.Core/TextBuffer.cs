using System;
using System.Collections.Generic;
using System.Linq;

namespace Yae.Core
{
    internal sealed class TextBuffer
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

        public void NewLine(int line, int index = -1)
        {
            var newLines = Enumerable.Repeat(string.Empty, Math.Max(line + 1 - _lines.Count, 0));
            foreach (var newLine in newLines)
            {
                _lines.Add(newLine);
            }

            if (index == -1)
            {
                _lines.Insert(line, string.Empty);
            }
            else
            {
                var copy = _lines[line];
                if (copy.Length < index + 1)
                {
                    _lines.Insert(line + 1, string.Empty);
                }
                else
                {
                    _lines[line] = _lines[line].Remove(index);
                    _lines.Insert(line + 1, copy.Substring(index));
                }
            }
        }

        public void RemoveLine(int line)
        {
            var newLines = Enumerable.Repeat(string.Empty, Math.Max(line + 1 - _lines.Count, 0));
            foreach (var newLine in newLines)
            {
                _lines.Add(newLine);
            }

            _lines.RemoveAt(line);
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

        public int GetLineLength(int line)
        {
            var newLines = Enumerable.Repeat(string.Empty, Math.Max(line + 1 - _lines.Count, 0));
            foreach (var newLine in newLines)
            {
                _lines.Add(newLine);
            }

            return _lines[line].Length;
        }
    }
}