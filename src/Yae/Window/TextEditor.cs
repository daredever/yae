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
            switch (inputKey.Key)
            {
                case ConsoleKey.Escape:
                    return true;
                case ConsoleKey.Backspace:
                    _linesBuffer.Remove(Cursor.Y + Offset, Cursor.X - 1);
                    Cursor = new Point(Cursor.X - 1, Cursor.Y);
                    _renderQueue.Enqueue(Cursor.Y);
                    return false;
                case ConsoleKey.Enter:
                    var y1 = Cursor.Y + 1;
                    if (Cursor.Y < _linesBuffer.Size - Offset)
                    {
                        if (y1 >= 20)
                        {
                            _linesBuffer.NewLine(Cursor.Y + Offset, Cursor.X);
                            Offset++;
                            Cursor = new Point(0, Cursor.Y);
                            for (var i = 0; i <= Cursor.Y; i++)
                            {
                                _renderQueue.Enqueue(i);
                            }
                        }
                        else
                        {
                            _linesBuffer.NewLine(Cursor.Y + Offset, Cursor.X);
                            for (var i = 19; i > Math.Max(Cursor.Y - 1, 0); i--)
                            {
                                _renderQueue.Enqueue(i);
                            }

                            _renderQueue.Enqueue(Cursor.Y);
                            _renderQueue.Enqueue(y1);
                            Cursor = new Point(0, y1);
                        }
                    }
                    else
                    {
                        _linesBuffer.NewLine(y1 + Offset, Cursor.X);
                        Cursor = new Point(0, y1);
                        _renderQueue.Enqueue(y1);
                    }

                    return false;
                case ConsoleKey.LeftArrow:
                    var x1 = Cursor.X - 1;
                    if (x1 < 0)
                    {
                        _renderQueue.Enqueue(Cursor.Y);
                    }
                    else
                    {
                        Cursor = new Point(x1, Cursor.Y);
                        _renderQueue.Enqueue(Cursor.Y);
                    }

                    return false;
                case ConsoleKey.UpArrow:
                    _renderQueue.Enqueue(Cursor.Y);
                    var y2 = Cursor.Y - 1;
                    if (y2 < 0)
                    {
                        if (y2 + Offset >= 0)
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
                        Cursor = new Point(Cursor.X, y2);
                        _renderQueue.Enqueue(y2);
                    }

                    return false;
                case ConsoleKey.RightArrow:
                    Cursor = new Point(Cursor.X + 1, Cursor.Y);
                    _renderQueue.Enqueue(Cursor.Y);

                    return false;
                case ConsoleKey.DownArrow:
                    _renderQueue.Enqueue(Cursor.Y);

                    var y3 = Cursor.Y + 1;
                    if (y3 >= _linesBuffer.Size - Offset)
                    {
                        return false;
                    }

                    if (y3 >= 20)
                    {
                        Offset++;
                        for (var i = 0; i <= Cursor.Y; i++)
                        {
                            _renderQueue.Enqueue(i);
                        }
                    }
                    else
                    {
                        Cursor = new Point(Cursor.X, y3);
                        _renderQueue.Enqueue(y3);
                    }

                    return false;
                case ConsoleKey.Delete:
                    _linesBuffer.Remove(Cursor.Y + Offset, Cursor.X);
                    _renderQueue.Enqueue(Cursor.Y);
                    return false;
                case ConsoleKey.End:
                    Cursor = new Point(_linesBuffer.GetLine(Cursor.Y).Length, Cursor.Y);
                    _renderQueue.Enqueue(Cursor.Y);
                    return false;
                case ConsoleKey.Home:
                    Cursor = new Point(0, Cursor.Y);
                    _renderQueue.Enqueue(Cursor.Y);
                    return false;
                case ConsoleKey.OemPlus:
                case ConsoleKey.OemComma:
                case ConsoleKey.OemMinus:
                case ConsoleKey.OemPeriod:
                case ConsoleKey.Oem1:
                case ConsoleKey.Oem2:
                case ConsoleKey.Oem3:
                case ConsoleKey.Oem4:
                case ConsoleKey.Oem5:
                case ConsoleKey.Oem6:
                case ConsoleKey.Oem7:
                case ConsoleKey.Oem8:
                case ConsoleKey.Oem102:
                case ConsoleKey.Spacebar:
                case ConsoleKey.D0:
                case ConsoleKey.D1:
                case ConsoleKey.D2:
                case ConsoleKey.D3:
                case ConsoleKey.D4:
                case ConsoleKey.D5:
                case ConsoleKey.D6:
                case ConsoleKey.D7:
                case ConsoleKey.D8:
                case ConsoleKey.D9:
                case ConsoleKey.A:
                case ConsoleKey.B:
                case ConsoleKey.C:
                case ConsoleKey.D:
                case ConsoleKey.E:
                case ConsoleKey.F:
                case ConsoleKey.G:
                case ConsoleKey.H:
                case ConsoleKey.I:
                case ConsoleKey.J:
                case ConsoleKey.K:
                case ConsoleKey.L:
                case ConsoleKey.M:
                case ConsoleKey.N:
                case ConsoleKey.O:
                case ConsoleKey.P:
                case ConsoleKey.Q:
                case ConsoleKey.R:
                case ConsoleKey.S:
                case ConsoleKey.T:
                case ConsoleKey.U:
                case ConsoleKey.V:
                case ConsoleKey.W:
                case ConsoleKey.X:
                case ConsoleKey.Y:
                case ConsoleKey.Z:
                case ConsoleKey.NumPad0:
                case ConsoleKey.NumPad1:
                case ConsoleKey.NumPad2:
                case ConsoleKey.NumPad3:
                case ConsoleKey.NumPad4:
                case ConsoleKey.NumPad5:
                case ConsoleKey.NumPad6:
                case ConsoleKey.NumPad7:
                case ConsoleKey.NumPad8:
                case ConsoleKey.NumPad9:
                case ConsoleKey.Multiply:
                case ConsoleKey.Add:
                case ConsoleKey.Subtract:
                case ConsoleKey.Decimal:
                case ConsoleKey.Divide:
                    _linesBuffer.Write(inputKey.KeyChar.ToString(), Cursor.Y + Offset, Cursor.X);
                    _renderQueue.Enqueue(Cursor.Y);
                    Cursor = new Point(Cursor.X + 1, Cursor.Y);
                    return false;
                case ConsoleKey.Tab:
                case ConsoleKey.PageUp:
                case ConsoleKey.PageDown:
                case ConsoleKey.Insert:
                case ConsoleKey.F1:
                case ConsoleKey.F2:
                case ConsoleKey.F3:
                case ConsoleKey.F4:
                case ConsoleKey.F5:
                case ConsoleKey.F6:
                case ConsoleKey.F7:
                case ConsoleKey.F8:
                case ConsoleKey.F9:
                case ConsoleKey.F10:
                case ConsoleKey.F11:
                case ConsoleKey.F12:
                case ConsoleKey.F13:
                case ConsoleKey.F14:
                case ConsoleKey.F15:
                case ConsoleKey.F16:
                case ConsoleKey.F17:
                case ConsoleKey.F18:
                case ConsoleKey.F19:
                case ConsoleKey.F20:
                case ConsoleKey.F21:
                case ConsoleKey.F22:
                case ConsoleKey.F23:
                case ConsoleKey.F24:
                case ConsoleKey.Separator:
                case ConsoleKey.Help:
                case ConsoleKey.LeftWindows:
                case ConsoleKey.RightWindows:
                case ConsoleKey.Applications:
                case ConsoleKey.Sleep:
                case ConsoleKey.Clear:
                case ConsoleKey.Pause:
                case ConsoleKey.Select:
                case ConsoleKey.Print:
                case ConsoleKey.Execute:
                case ConsoleKey.PrintScreen:
                case ConsoleKey.BrowserBack:
                case ConsoleKey.BrowserForward:
                case ConsoleKey.BrowserRefresh:
                case ConsoleKey.BrowserStop:
                case ConsoleKey.BrowserSearch:
                case ConsoleKey.BrowserFavorites:
                case ConsoleKey.BrowserHome:
                case ConsoleKey.VolumeMute:
                case ConsoleKey.VolumeDown:
                case ConsoleKey.VolumeUp:
                case ConsoleKey.MediaNext:
                case ConsoleKey.MediaPrevious:
                case ConsoleKey.MediaStop:
                case ConsoleKey.MediaPlay:
                case ConsoleKey.LaunchMail:
                case ConsoleKey.LaunchMediaSelect:
                case ConsoleKey.LaunchApp1:
                case ConsoleKey.LaunchApp2:
                case ConsoleKey.Process:
                case ConsoleKey.Packet:
                case ConsoleKey.Attention:
                case ConsoleKey.CrSel:
                case ConsoleKey.ExSel:
                case ConsoleKey.EraseEndOfFile:
                case ConsoleKey.Play:
                case ConsoleKey.Zoom:
                case ConsoleKey.NoName:
                case ConsoleKey.Pa1:
                case ConsoleKey.OemClear:
                default:
                    _renderQueue.Enqueue(Cursor.Y);
                    return false;
            }
        }
    }
}