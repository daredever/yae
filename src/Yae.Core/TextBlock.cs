using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yae.Core
{
    internal sealed class TextBlock
    {
        private readonly int _linesPerPage;
        private readonly TextBuffer _textBuffer;
        private readonly Cursor _cursor;

        private int _previousCursorX = 0;

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
            for (var row = 0; row < Math.Min(_linesPerPage, _textBuffer.Size); row++)
            {
                var line = row + _cursor.Offset;
                var value = _textBuffer.GetLine(line);
                lines.Add(new Line(line, value, row));
            }

            return lines;
        }

        public IEnumerable<string> GetAllLines()
        {
            for (var line = 0; line < _textBuffer.Size; line++)
            {
                yield return _textBuffer.GetLine(line);
            }
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
            _previousCursorX = _cursor.X;
        }

        public void HandleEnd()
        {
            _cursor.X = _textBuffer.GetLineLength(_cursor.AbsoluteY);
            _previousCursorX = _cursor.X;
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
            }
            else
            {
                _cursor.Y++;
            }

            var lineLength = _textBuffer.GetLineLength(_cursor.AbsoluteY);
            _cursor.X = Math.Clamp(_cursor.X, Math.Min(_previousCursorX, lineLength), lineLength);
        }

        public void HandleUpArrow()
        {
            if (_cursor.Y - 1 < 0)
            {
                if (_cursor.AbsoluteY - 1 >= 0)
                {
                    _cursor.Offset--;
                }
            }
            else
            {
                _cursor.Y--;
            }

            var lineLength = _textBuffer.GetLineLength(_cursor.AbsoluteY);
            _cursor.X = Math.Clamp(_cursor.X, Math.Min(_previousCursorX, lineLength), lineLength);
        }

        public void HandleRightArrow()
        {
            if (_cursor.X < _textBuffer.GetLineLength(_cursor.AbsoluteY))
            {
                _cursor.X++;
            }

            _previousCursorX = _cursor.X;
        }

        public void HandleLeftArrow()
        {
            if (_cursor.X - 1 < 0)
            {
                return;
            }

            _cursor.X--;
            _previousCursorX = _cursor.X;
        }

        public void HandleEnter()
        {
            if (_cursor.AbsoluteY < _textBuffer.Size)
            {
                _textBuffer.NewLine(_cursor.AbsoluteY, _cursor.X);
                if (_cursor.Y + 1 >= _linesPerPage)
                {
                    _cursor.Offset++;
                }
                else
                {
                    _cursor.Y++;
                }
            }
            else
            {
                _cursor.Y++;
                _textBuffer.NewLine(_cursor.AbsoluteY, 0);
            }

            _cursor.X = 0;
            _previousCursorX = _cursor.X;
        }

        public void HandleBackspace()
        {
            if (_cursor.X - 1 < 0)
            {
                return;
            }

            _cursor.X--;
            _textBuffer.Remove(_cursor.AbsoluteY, _cursor.X);
        }

        public void HandleDelete()
        {
            if (_cursor.X >= _textBuffer.GetLineLength(_cursor.AbsoluteY))
            {
                return;
            }

            _textBuffer.Remove(_cursor.AbsoluteY, _cursor.X);
        }
    }
}