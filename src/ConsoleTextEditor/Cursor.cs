namespace ConsoleTextEditor
{
    internal sealed class Cursor
    {
        public Cursor(int x, int y, int offset)
        {
            X = x;
            Y = y;
            Offset = offset;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Offset { get; set; }

        public int AbsoluteY => Y + Offset;
    }
}