using System.Collections.Generic;

namespace Booster.CodingTest.MarkBenson.Models
{
    public class MultiStat: Stat
    {
        public IEnumerable<string> Data { get; }

        public MultiStat(string statName, IEnumerable<string> data) : base(statName)
        {
            Data = data;
        }
    }
}