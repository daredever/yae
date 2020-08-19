using System;

namespace Yae.Core.Validators
{
    internal static class LinesCountValidator
    {
        private const int MinLinesCount = 1;
        private const int MaxLinesCount = 100;

        public static void Validate(int linesPerPage)
        {
            if (linesPerPage < MinLinesCount)
            {
                throw new ArgumentException($"Lines per page could not be less than {MinLinesCount}",
                    nameof(linesPerPage));
            }

            if (linesPerPage > MaxLinesCount)
            {
                throw new ArgumentException($"Lines per page could not be greater than {MaxLinesCount}",
                    nameof(linesPerPage));
            }
        }
    }
}