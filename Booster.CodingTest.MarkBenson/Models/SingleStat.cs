namespace Booster.CodingTest.MarkBenson.Models
{
    public class SingleStat : Stat
    {
        public string Data { get; }

        public SingleStat(string statName, string data) : base(statName)
        {
            Data = data;
        }
    }
}