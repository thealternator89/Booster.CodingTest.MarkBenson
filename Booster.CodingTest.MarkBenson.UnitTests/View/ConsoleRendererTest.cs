using System.Linq;
using Booster.CodingTest.MarkBenson.Models;
using Booster.CodingTest.MarkBenson.View;
using Xunit;

namespace Booster.CodingTest.MarkBenson.UnitTests.View
{
    public class ConsoleRendererTest
    {
        [Fact]
        public void FormatSingleStat_BuildsLinesOfMaxWidth()
        {
            const int maxWidth = 300;
            var lines = ConsoleRenderer.FormatSingleStat(new SingleStat("Test", "Data"), 15, maxWidth);
            foreach (var line in lines)
            {
                Assert.Equal(maxWidth, line.Length);
            }
        }

        [Fact]
        public void FormatSingleStat_PadsStatName()
        {
            var lines = ConsoleRenderer.FormatSingleStat(new SingleStat("Test", "Data"), 10, 30);
            Assert.StartsWith("Test:     ", lines.First());
        }

        [Fact]
        public void FormatSingleStat_WrapsLongString()
        {
            var lines = ConsoleRenderer.FormatSingleStat(new SingleStat("Test", "123456789012345678901234567"), 6, 16);
            Assert.Equal(3, lines.Count());
        }

        [Fact]
        public void FormatSingleStat_HandlesStringWithExactMultipleOfWidthCorrectly()
        {
            var lines = ConsoleRenderer.FormatSingleStat(new SingleStat("Test", "12345678901234567890"), 6, 16);
            Assert.Equal(2, lines.Count());
        }

        [Fact]
        public void FormatMultiStat_BuildsLinesOfMaxWidth()
        {
            const int maxWidth = 300;
            var lines = ConsoleRenderer.FormatMultiStat(new MultiStat("Test", new[]{"Data"}), 15, maxWidth);
            foreach (var line in lines)
            {
                Assert.Equal(maxWidth, line.Length);
            }
        }

        [Fact]
        public void FormatMultiStat_WrapsItems()
        {
            var lines = ConsoleRenderer.FormatMultiStat(new MultiStat("Test", new[]{"one", "two", "three", "four", "five"}), 6, 16).ToList();
            Assert.Equal(3, lines.Count);
            Assert.StartsWith("      three, ", lines[1]);
        }

        [Fact]
        public void FormatMultiStat_DoesntPostfixCommaForFinalItem()
        {
            var lines = ConsoleRenderer.FormatMultiStat(new MultiStat("Test", new[]{"one", "two"}), 6, 16);
            Assert.Equal("Test: one, two  ", lines.First());
        }
    }
}