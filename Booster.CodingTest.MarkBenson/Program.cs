using System.Linq;
using Booster.CodingTest.MarkBenson.Data;
using Booster.CodingTest.MarkBenson.Models;
using Booster.CodingTest.MarkBenson.Store;
using Booster.CodingTest.MarkBenson.View;

namespace Booster.CodingTest.MarkBenson
{
    class Program
    {
        static void Main(string[] args)
        {
            using var stream = new WordsStream();
            var stats = new StatsManager();
            var console = new ConsoleRenderer();

            while (true)
            {
                stats.ProcessWordStats(stream.ReadWord());
                console.RenderStats(new Stat[]
                {
                    new SingleStat("Word Count", stats.TotalWords.ToString()),
                    new SingleStat("Char Count", stats.TotalChars.ToString()),
                    new MultiStat("5 Largest", stats.FiveLongestWords),
                    new MultiStat("5 Smallest", stats.FiveShortestWords),
                    new MultiStat("10 Most Frequent", stats.TenMostFrequentWords.Select(item => item.ToString())),
                    new MultiStat("Char Frequency", stats.AllChars.Select(item => item.ToString())),
                });
            }
        }
    }
}