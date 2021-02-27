using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Booster.CodingTest.MarkBenson.Models;

namespace Booster.CodingTest.MarkBenson.View
{
    public class ConsoleRenderer
    {
        private int _lastLineCount;

        /// <summary>
        /// Render the provided stats to the console, overwriting any existing content previously rendered by this renderer.
        /// </summary>
        /// <param name="stats">Stats to render</param>
        public void RenderStats(IEnumerable<Stat> stats)
        {
            // On Windows, the width is 1 character wider than the console
            var consoleWidth = Console.WindowWidth - 1;
            var lines = BuildOutputFromStats(stats, consoleWidth);
            
            // Return the caret to the top and render the new content
            ReturnCaret();
            lines.ForEach(Console.WriteLine);

            // If we have written more lines than last time, update the variable,
            // Otherwise erase the remaining lines with spaces
            if (lines.Count > _lastLineCount)
            {
                _lastLineCount = lines.Count;
            }
            else
            {
                FillLines(_lastLineCount- lines.Count, consoleWidth);
            }
        }

        /// <summary>
        /// Build some output from the provided stats
        /// </summary>
        /// <param name="stats">Stats to build output text for</param>
        /// <param name="consoleWidth">The max width to render lines</param>
        /// <returns>A list of lines to print to console</returns>
        private static List<string> BuildOutputFromStats(IEnumerable<Stat> stats, int consoleWidth)
        {
            var statsList = stats.ToList();

            // Find the max length of stat name, and add 2 (for ": ")
            var titleWidth = statsList.Select(stat => stat.StatName)
                .Aggregate(0, (prev, current) => current.Length > prev ? current.Length : prev) + 2;

            var lines = new List<string>();

            foreach (var stat in statsList)
            {
                switch (stat)
                {
                    case SingleStat single:
                        lines.AddRange(FormatSingleStat(single, titleWidth, consoleWidth));
                        break;
                    case MultiStat multi:
                        lines.AddRange(FormatMultiStat(multi, titleWidth, consoleWidth));
                        break;
                }
            }

            return lines;
        }

        /// <summary>
        /// Build a set of lines for rendering a SingleStat (a stat containing a single distinct value)
        /// If this extends beyond the width of the console, it will be wrapped with no concern of the value.
        /// </summary>
        /// <param name="stat">Stat to render</param>
        /// <param name="titleWidth">The width to use for the title</param>
        /// <param name="maxWidth">The maximum width of any line</param>
        /// <returns>An enumerable collection of lines to print to console</returns>
        public static IEnumerable<string> FormatSingleStat(SingleStat stat, int titleWidth, int maxWidth)
        {
            var lineWidth = maxWidth - titleWidth;
            var lines = SplitByLength(stat.Data, lineWidth).Select((line) => PadRight(line, lineWidth)).ToList();
            return FormatLines(stat.StatName, titleWidth, maxWidth, lines);
        }

        /// <summary>
        /// Build a set of lines for rendering a MultiStat (a stat containing multiple distinct values)
        /// This takes into account the console size, etc. and splits the stat into values which aren't broken by linebreaks
        /// </summary>
        /// <param name="stat">Stat to render</param>
        /// <param name="titleWidth">The width to use for the title</param>
        /// <param name="maxWidth">The maximum width of any line</param>
        /// <returns>An enumerable collection of lines to print to console</returns>
        public static IEnumerable<string> FormatMultiStat(MultiStat stat, int titleWidth, int maxWidth)
        {
            var dataArr = stat.Data.ToArray();
            var lineWidth = maxWidth - titleWidth;

            var lines = BuildMultiStatLines(dataArr, lineWidth);

            return FormatLines(stat.StatName, titleWidth, maxWidth, lines);
        }

        /// <summary>
        /// Build some text to render a multi-stat which is formatted so items aren't broken by linebreaks
        /// </summary>
        /// <param name="dataArr">Items in the stat</param>
        /// <param name="lineWidth">Max line length before folding</param>
        /// <returns>A list of lines of text for the multi stat</returns>
        private static List<string> BuildMultiStatLines(IReadOnlyList<string> dataArr, int lineWidth)
        {
            var lines = new List<string>();
            var lineBuilder = new StringBuilder();

            for (var i = 0; i < dataArr.Count; i++)
            {
                var item = dataArr[i];

                // Postfix item with a comma if it isn't the last one.
                if (i != dataArr.Count - 1)
                {
                    item += ",";
                }
                
                // Current item will push us over the line width limit
                // Add the current line to the set of lines and create a new builder
                if (lineBuilder.Length + item.Length + 1 > lineWidth)
                {
                    // NOTE: If we would ever get an item which exceeds the console width, we would need to add a check to split this up
                    // Not handling this case as part of this exercise, as the longest words are ~12 characters.
                    lines.Add(PadRight(lineBuilder.ToString(), lineWidth));
                    lineBuilder = new StringBuilder();
                }
                // We're on the same line, prefix a space to the item to separate it nicely
                else if (i != 0)
                {
                    item = " " + item;
                }

                lineBuilder.Append(item);
            }

            lines.Add(PadRight(lineBuilder.ToString(), lineWidth));
            return lines;
        }

        /// <summary>
        /// Generic formatting which happens whether we are rendering a single or multi stat.
        /// </summary>
        /// <param name="title">The title text to display</param>
        /// <param name="titleWidth">The width for the title text - to keep all titles aligned</param>
        /// <param name="width">Console width</param>
        /// <param name="lines">Lines to format</param>
        /// <returns>Formatted lines</returns>
        private static IEnumerable<string> FormatLines(string title, int titleWidth, int width, List<string> lines)
        {
            // Add title to the first line
            lines[0] = PadRight($"{title}: ", titleWidth) + lines[0];
            // Left pad all lines to be the console width - this will push all but the first line over to the right
            return lines.Select((line) => PadLeft(line, width));
        }
        
        /// <summary>
        /// Split the provided string by a set length
        /// </summary>
        /// <param name="text">The text to split</param>
        /// <param name="maxLength">The maximum length of any line</param>
        /// <returns>An enumerable collection of lines with no string longer than maxLength</returns>
        private static IEnumerable<string> SplitByLength(string text, int maxLength) {
            for (var i = 0; i < text.Length; i += maxLength) {
                yield return text.Substring(i, Math.Min(maxLength, text.Length - i));
            }
        }

        /// <summary>
        /// Returns the caret to the beginning of the last input.
        /// This assumes the console shape hasn't changed since it was last rendered
        /// </summary>
        private void ReturnCaret()
        {
            Console.CursorTop -= _lastLineCount;
        }

        /// <summary>
        /// Adds padding to the right side of the input string so it is at least the specified width.
        /// This will only return a longer string if one is provided.
        /// </summary>
        /// <param name="text">String to pad</param>
        /// <param name="width">Minimum width to make string</param>
        /// <returns>The input string with spaces added to the right so it is the specified width</returns>
        private static string PadRight(string text, int width)
        {
            var padding = width - text.Length;
            return padding > 0 ? text + Spaces(padding) : text;
        }
        
        /// <summary>
        /// Adds padding to the left side of the input string so it is at least the specified width.
        /// This will only return a longer string if one is provided.
        /// </summary>
        /// <param name="text">String to pad</param>
        /// <param name="width">Minimum width to make string</param>
        /// <returns>The input string with spaces added to the left so it is the specified width</returns>
        private static string PadLeft(string text, int width)
        {
            var padding = width - text.Length;
            return padding > 0 ? Spaces(padding) + text : text;
        }
        
        /// <summary>
        /// Prints the specified number of lines with spaces to erase any existing content on the lines.
        /// This assumes that the caret is above existing content on the console, otherwise it will print a series of blank lines.
        /// </summary>
        /// <param name="lines">Number of lines to fill</param>
        /// <param name="width">Width of console to fill</param>
        private static void FillLines(int lines, int width)
        {
            for (var i = 0; i < lines; i++)
            {
                Console.WriteLine(Spaces(width));
            }
        }

        /// <summary>
        /// Create a new string of the specified length with spaces.
        /// </summary>
        /// <param name="count">Required string length</param>
        /// <returns>A string of the specified length, consisting of spaces</returns>
        private static string Spaces(int count)
        {
            return string.Join(' ', new string[count + 1]);
        }
    }
}