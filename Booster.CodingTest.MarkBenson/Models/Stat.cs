namespace Booster.CodingTest.MarkBenson.Models
{
    public abstract class Stat
    {
        public string StatName { get; set; }

        protected Stat(string statName)
        {
            StatName = statName;
        }
    }
}