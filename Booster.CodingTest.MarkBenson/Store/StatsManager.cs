using System;
using System.Collections.Generic;
using System.Linq;

using Booster.CodingTest.MarkBenson.Models;

namespace Booster.CodingTest.MarkBenson.Store
{
    public class StatsManager
    {
        public int TotalChars => GetTotalFrequency(_charFrequency);
        public int TotalWords => GetTotalFrequency(_wordFrequency);
        
        public IEnumerable<string> FiveLongestWords =>
            _wordFrequency.Keys.ToList().OrderByDescending((word) => word.Length).Take(5);
        public IEnumerable<string> FiveShortestWords =>
            _wordFrequency.Keys.ToList().OrderBy((word) => word.Length).Take(5);

        public IEnumerable<ObjectFrequency<string>> TenMostFrequentWords =>
            _wordFrequency.Values.ToList().OrderByDescending((word) => word.Frequency).Take(10);

        private readonly Dictionary<string, ObjectFrequency<string>> _wordFrequency = new();
        private readonly Dictionary<char, ObjectFrequency<char>> _charFrequency = new();
        
        public void ProcessWordStats(string word)
        {
            AddOrIncrement(word, _wordFrequency);
            foreach (var character in word)
            {
                AddOrIncrement(character, _charFrequency);
            }
        }
        
        private static void AddOrIncrement<T>(T key, IDictionary<T, ObjectFrequency<T>> dict)
        {
            if (dict.ContainsKey(key))
            {
                dict[key].Frequency++;
            }
            else
            {
                dict.Add(key, new ObjectFrequency<T>(key));
            }
        }
        
        private static int GetTotalFrequency<T>(Dictionary<T, ObjectFrequency<T>> dictionary)
        {
            return dictionary.Values.ToList().Select(v => v.Frequency)
                .Aggregate(0, (current, item) => current + item);
        }
    }
}