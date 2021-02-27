using System.Text.RegularExpressions;
using Booster.CodingTest.MarkBenson.Data;
using Xunit;

namespace Booster.CodingTest.MarkBenson.UnitTests.Data
{
    public class WordsStreamTest
    {
        [Fact]
        public void GeneratedWordsOnlyContainAlphaChars()
        {
            using var wordsStream = new WordsStream();
            var regex = new Regex("^[a-z]+$", RegexOptions.IgnoreCase);
            // Check 100 times - this isn't guaranteed to be stable, but should fail more easily than if we just tested one
            for (var i = 0; i < 100; i++)
            {
                Assert.Matches(regex, wordsStream.ReadWord());
            }
        }

        [Fact]
        public void GeneratedWordsAreLowerCase()
        {
            using var wordsStream = new WordsStream();
            
            // Check 100 times - this isn't guaranteed to be stable, but should fail more easily than if we just tested one
            for (var i = 0; i < 100; i++)
            {
                var word = wordsStream.ReadWord();
                Assert.Equal(word.ToLower(), word);
            }
        }

        [Fact]
        public void GeneratedWordsAreNonZeroLength()
        {
            using var wordsStream = new WordsStream();
            
            // Check 100 times - A sentence should have ended within 100 words
            for (var i = 0; i < 100; i++)
            {
                Assert.True(wordsStream.ReadWord().Length > 0);
            }
        }
    }
}