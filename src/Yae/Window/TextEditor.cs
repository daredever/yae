using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Yae.Buffer;
using Yae.Utils;

namespace Yae.Window
{
    internal sealed class TextEditor
    {
        private readonly FileInfo _file;
        private readonly Header _header;
        private readonly Body _body;
        private readonly Footer _footer;

        private readonly LinesBuffer _linesBuffer;
        private readonly Queue<int> _renderQueue = new Queue<int>();

        private Point Cursor = Point.Empty;
        private int Offset = 0;

        public TextEditor(FileInfo file, int linesPerPage = 20)
        {
            _file = file ?? throw new ArgumentNullException(nameof(file));
            _header = new Header(_file);
            _body = new Body(linesPerPage);
            _footer = new Footer();

            _linesBuffer = new LinesBuffer();
        }

        private void EnsureWindowSize()
        {
        }

        public async Task RunAsync()
        {
            Output.ClearScreen();
            EnsureWindowSize();
            Cursor = new Point(0, 0);

            await _header.RenderAsync();
            await _body.RenderAsync();
            await _footer.RenderAsync();

            var inputData = await File.ReadAllLinesAsync(_file.FullName);
            var y = 0;
            foreach (var s in inputData)
            {
                _linesBuffer.Write(s, y + Offset, 0);
                _renderQueue.Enqueue(y);
                y++;
            }

            var x = _linesBuffer.GetLine(_linesBuffer.Size - 1).Length;
            Cursor = new Point(x, Cursor.Y);
            Output.MoveCursor(x + 9, 3);

            try
            {
                while (true)
                {
                    EnsureWindowSize();
                    await RenderAsync();
                    if (await WaitInputKeyAsync())
                    {
                        Output.ClearScreen();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Output.ClearScreen();

                // todo move cursor to bottom 
                Output.MoveCursor(0, 0);
                await Output.WriteLineAsync(ex.Message);
            }
        }

        private async Task RenderAsync()
        {
            while (_renderQueue.TryDequeue(out var line))
            {
                Cursor = new Point(Cursor.X, line);

                Output.HideCursor();
                Output.MoveCursor(0, Cursor.Y + 3);

                await _body.RenderLineAsync(_linesBuffer.GetLine(line + Offset), line + Offset + 1);

                Output.MoveCursor(Cursor.X + 9, Cursor.Y + 3);
                Output.ShowCursor();
            }
        }

        private async Task<bool> WaitInputKeyAsync()
        {
            var inputKey = Console.ReadKey();

            if (inputKey.Key == ConsoleKey.Escape)
            {
                return true;
            }

            if (inputKey.Key == ConsoleKey.Enter)
            {
                var y = Cursor.Y + 1;
                if (Cursor.Y < _linesBuffer.Size - Offset)
                {
                    if (y >= 20)
                    {
                        _linesBuffer.NewLine(Cursor.Y + Offset);
                        Offset++;
                        Cursor = new Point(0, Cursor.Y);
                        for (var i = 0; i <= Cursor.Y; i++)
                        {
                            _renderQueue.Enqueue(i);
                        }
                    }
                    else
                    {
                        _linesBuffer.NewLine(Cursor.Y + Offset);
                        for (var i = 19; i > Math.Max(Cursor.Y - 1, 0); i--)
                        {
                            _renderQueue.Enqueue(i);
                        }

                        _renderQueue.Enqueue(Cursor.Y);
                        _renderQueue.Enqueue(y);
                        Cursor = new Point(0, y);
                    }
                }
                else
                {
                    _linesBuffer.NewLine(y + Offset);
                    Cursor = new Point(0, y);
                    _renderQueue.Enqueue(y);
                }

                return false;
            }

            if (inputKey.Key == ConsoleKey.Backspace)
            {
                _linesBuffer.Remove(Cursor.Y + Offset, Cursor.X - 1);
                Cursor = new Point(Cursor.X - 1, Cursor.Y);
                _renderQueue.Enqueue(Cursor.Y);

                return false;
            }

            if (inputKey.Key == ConsoleKey.Delete)
            {
                _linesBuffer.Remove(Cursor.Y + Offset, Cursor.X);
                _renderQueue.Enqueue(Cursor.Y);

                return false;
            }

            if (inputKey.Key == ConsoleKey.UpArrow)
            {
                _renderQueue.Enqueue(Cursor.Y);
                var y = Cursor.Y - 1;
                if (y < 0)
                {
                    if (y + Offset >= 0)
                    {
                        Offset--;
                        for (var i = 19; i >= 0; i--)
                        {
                            _renderQueue.Enqueue(i);
                        }
                    }
                }
                else
                {
                    Cursor = new Point(Cursor.X, y);
                    _renderQueue.Enqueue(y);
                }

                return false;
            }

            if (inputKey.Key == ConsoleKey.DownArrow)
            {
                _renderQueue.Enqueue(Cursor.Y);

                var y = Cursor.Y + 1;
                if (y >= _linesBuffer.Size - Offset)
                {
                    return false;
                }

                if (y >= 20)
                {
                    Offset++;
                    for (var i = 0; i <= Cursor.Y; i++)
                    {
                        _renderQueue.Enqueue(i);
                    }
                }
                else
                {
                    Cursor = new Point(Cursor.X, y);
                    _renderQueue.Enqueue(y);
                }

                return false;
            }

            if (inputKey.Key == ConsoleKey.LeftArrow)
            {
                var x = Cursor.X - 1;
                if (x < 0)
                {
                    _renderQueue.Enqueue(Cursor.Y);
                }
                else
                {
                    Cursor = new Point(x, Cursor.Y);
                    _renderQueue.Enqueue(Cursor.Y);
                }

                return false;
            }

            if (inputKey.Key == ConsoleKey.RightArrow)
            {
                Cursor = new Point(Cursor.X + 1, Cursor.Y);
                _renderQueue.Enqueue(Cursor.Y);

                return false;
            }

            _linesBuffer.Write(inputKey.KeyChar.ToString(), Cursor.Y + Offset, Cursor.X);
            _renderQueue.Enqueue(Cursor.Y);
            Cursor = new Point(Cursor.X + 1, Cursor.Y);

            return false;
        }
    }
}