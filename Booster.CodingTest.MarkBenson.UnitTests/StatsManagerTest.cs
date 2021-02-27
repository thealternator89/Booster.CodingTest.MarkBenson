using System.Collections.Generic;
using System.Linq;
using Booster.CodingTest.MarkBenson.Data;
using Xunit;

namespace Booster.CodingTest.MarkBenson.UnitTests
{
    public class StatsManagerTest
    {
        [Theory]
        [InlineData(new string[0], 0)]
        [InlineData(new[]{"one"}, 1)]
        [InlineData(new[]{"1", "2", "3", "4", "5"}, 5)]
        public void TotalWords(IEnumerable<string> input, int expectedCount)
        {
            var stats = CreateAndFillStatsManager(input);
            Assert.Equal(expectedCount, stats.TotalWords);
        }

        [Theory]
        [InlineData(new string[0], 0)]
        [InlineData(new[]{"one"}, 3)]
        [InlineData(new[]{"one", "two", "three", "four", "five"}, 19)]
        public void TotalChars(IEnumerable<string> input, int expectedCount)
        {
            var stats = CreateAndFillStatsManager(input);
            Assert.Equal(expectedCount, stats.TotalChars);
        }

        [Theory]
        [InlineData(new string[0], new string[0])]
        [InlineData(new[]{"1", "123456", "123", "12", "12", "1", "1234", "123456", "12345"},
            new[]{"123456", "12345", "1234", "123", "12"})]
        public void FiveLongestWords(IEnumerable<string> input, IEnumerable<string> expectedWords)
        {
            var stats = CreateAndFillStatsManager(input);
            Assert.Equal(expectedWords, stats.FiveLongestWords);
        }

        [Theory]
        [InlineData(new string[0], new string[0])]
        public void FiveShortestWords(IEnumerable<string> input, IEnumerable<string> expectedWords)
        {
            var stats = CreateAndFillStatsManager(input);
            Assert.Equal(expectedWords, stats.FiveShortestWords);
        }

        [Theory]
        [InlineData(new string[0], new string[0])]
        [InlineData(new[]{"one", "two", "three", "two", "four", "three", "four", "three", "four", "four"},
            new[]{"four", "three", "two", "one"})]
        [InlineData(new[]
        {
            "ten",
            "ten", "nine",
            "ten", "nine", "eight",
            "ten", "nine", "eight", "seven",
            "ten", "nine", "eight", "seven", "six", 
            "ten", "nine", "eight", "seven", "six", "five", 
            "ten", "nine", "eight", "seven", "six", "five", "four", 
            "ten", "nine", "eight", "seven", "six", "five", "four", "three", 
            "ten", "nine", "eight", "seven", "six", "five", "four", "three", "two",
            "ten", "nine", "eight", "seven", "six", "five", "four", "three", "two", "one",
            "ten", "nine", "eight", "seven", "six", "five", "four", "three", "two", "one", "zero"
        },
            new[]{"ten", "nine", "eight", "seven", "six", "five", "four", "three", "two", "one"})]
        public void TenMostFrequentWords(IEnumerable<string> input, IEnumerable<string> expectedWords)
        {
            var stats = CreateAndFillStatsManager(input);
            Assert.Equal(expectedWords, stats.TenMostFrequentWords.Select(w => w.Key));
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("zero")]
        [InlineData("0")]
        public void ExistingWordsTakePrecedenceOverNewOnesWithSameFrequency(string newWord)
        {
            var stats = CreateAndFillStatsManager(new[]
                {"ten", "nine", "eight", "seven", "six", "five", "four", "three", "two", "one"});
            stats.ProcessWordStats(newWord);
            Assert.DoesNotContain(stats.TenMostFrequentWords, (w) => w.Key == newWord);
        }

        private static StatsManager CreateAndFillStatsManager(IEnumerable<string> wordsToFill)
        {
            var stats = new StatsManager();
            foreach (var word in wordsToFill)
            {
                stats.ProcessWordStats(word);
            }

            return stats;
        }
    }
}