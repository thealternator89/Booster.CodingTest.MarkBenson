using System.Linq;
using System.Threading;
using Booster.CodingTest.MarkBenson.Data;
using Booster.CodingTest.MarkBenson.Models;
using Booster.CodingTest.MarkBenson.Store;
using Booster.CodingTest.MarkBenson.View;

namespace Booster.CodingTest.MarkBenson.Controller
{
    public static class ConsoleController
    {
        public static void Run(int sleepTime)
        {
            using var stream = new WordsStream();
            var stats = new StatsManager();
            var renderer = new ConsoleRenderer();
            
            while (true)
            {
                stats.ProcessWordStats(stream.ReadWord());
                renderer.RenderStats(new Stat[]
                {
                    new SingleStat("Word Count", stats.TotalWords.ToString()),
                    new SingleStat("Char Count", stats.TotalChars.ToString()),
                    new MultiStat("Five Largest", stats.FiveLongestWords),
                    new MultiStat("Five Smallest", stats.FiveShortestWords),
                    new MultiStat("Ten Most Frequent", stats.TenMostFrequentWords.Select(item => item.ToString())),
                    new MultiStat("Char Frequency", stats.AllChars.Select(item => item.ToString())),
                });
                if (sleepTime > 0)
                {
                    Thread.Sleep(sleepTime);
                }
            }
        }
    }
}