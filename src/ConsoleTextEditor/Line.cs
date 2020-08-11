namespace ConsoleTextEditor
{
    internal sealed class Line
    {
        public Line(int number, string value, int row)
        {
            Number = number;
            Value = value;
            Row = row;
        }

        public int Row { get; }

        public int Number { get; }

        public string Value { get; }
    }
}