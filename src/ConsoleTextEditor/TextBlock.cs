using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleTextEditor
{
    internal sealed class TextBlock
    {
        private readonly int _linesPerPage;
        private readonly TextBuffer _textBuffer;
        private readonly Cursor _cursor;

        public TextBlock(int linesPerPage)
        {
            _linesPerPage = linesPerPage;
            _cursor = new Cursor(0, 0, 0);
            _textBuffer = new TextBuffer();
        }

        public int CursorX => _cursor.X;

        public int CursorY => _cursor.Y;

        public Task InitAsync(IEnumerable<string> input)
        {
            var cursorY = 0;
            foreach (var value in input)
            {
                var absoluteY = cursorY + _cursor.Offset;
                _textBuffer.Write(value, absoluteY, 0);
                cursorY++;
            }

            return Task.CompletedTask;
        }

        // TODO get only changed lines
        public IEnumerable<Line> GetChangedLines()
        {
            var lines = new List<Line>(_linesPerPage);
            for (var row = 0; row < _linesPerPage; row++)
            {
                var line = row + _cursor.Offset;
                var value = _textBuffer.GetLine(line);
                lines.Add(new Line(line, value, row));
            }

            return lines;
        }

        public void HandleSkippedKeys(ConsoleKeyInfo inputKey)
        {
        }

        public void HandleAlphabet(ConsoleKeyInfo inputKey)
        {
            _textBuffer.Write(inputKey.KeyChar.ToString(), _cursor.AbsoluteY, _cursor.X);
            _cursor.X++;
        }

        public void HandleHome()
        {
            _cursor.X = 0;
        }

        public void HandleEnd()
        {
            _cursor.X = _textBuffer.GetLineLength(_cursor.AbsoluteY);
        }

        public void HandleDelete()
        {
            _textBuffer.Remove(_cursor.AbsoluteY, _cursor.X);
        }

        public void HandleDownArrow()
        {
            if (_cursor.AbsoluteY + 1 >= _textBuffer.Size)
            {
                return;
            }

            if (_cursor.Y + 1 >= _linesPerPage)
            {
                _cursor.Offset++;
                return;
            }

            _cursor.Y++;
        }

        public void HandleRightArrow()
        {
            if (_cursor.X < _textBuffer.GetLineLength(_cursor.AbsoluteY))
            {
                _cursor.X++;
            }
        }

        public void HandleUpArrow()
        {
            if (_cursor.Y - 1 < 0)
            {
                if (_cursor.AbsoluteY - 1 >= 0)
                {
                    _cursor.Offset--;
                }

                return;
            }

            _cursor.Y--;
        }

        public void HandleLeftArrow()
        {
            if (_cursor.X - 1 < 0)
            {
                return;
            }

            _cursor.X--;
        }

        public void HandleEnter()
        {
            if (_cursor.AbsoluteY < _textBuffer.Size)
            {
                if (_cursor.Y + 1 >= _linesPerPage)
                {
                    _textBuffer.NewLine(_cursor.AbsoluteY, _cursor.X);
                    _cursor.Offset++;
                    _cursor.X = 0;
                }
                else
                {
                    _textBuffer.NewLine(_cursor.AbsoluteY, _cursor.X);
                    _cursor.X = 0;
                    _cursor.Y++;
                }
            }
            else
            {
                _cursor.X = 0;
                _cursor.Y++;
                _textBuffer.NewLine(_cursor.AbsoluteY, _cursor.X);
            }
        }

        public void HandleBackspace()
        {
            _cursor.X--;
            _textBuffer.Remove(_cursor.AbsoluteY, _cursor.X);
        }
    }
}