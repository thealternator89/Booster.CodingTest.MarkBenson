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

        public IEnumerable<ObjectFrequency<char>> AllChars =>
            _charFrequency.Values.ToList().OrderByDescending((o) => o.Frequency);

        private readonly Dictionary<string, ObjectFrequency<string>> _wordFrequency = new();
        private readonly Dictionary<char, ObjectFrequency<char>> _charFrequency = new();
        
        /// <summary>
        /// Store the provided word for use in stats
        /// </summary>
        /// <param name="word">The word to store</param>
        public void ProcessWordStats(string word)
        {
            AddOrIncrement(word, _wordFrequency);
            foreach (var character in word)
            {
                AddOrIncrement(character, _charFrequency);
            }
        }
        
        /// <summary>
        /// Helper method to either add the specified item, or increment it in the provided dictionary
        /// </summary>
        /// <param name="key">Item to add or increment</param>
        /// <param name="dict">Dictionary to add or increment the item in</param>
        /// <typeparam name="T">The type of item and key in provided dictionary</typeparam>
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
        
        /// <summary>
        /// Gets the total frequency of items in the provided dictionary
        /// </summary>
        /// <param name="dictionary">Dictionary to get frequency from</param>
        /// <typeparam name="T">Type of items</typeparam>
        /// <returns>The total frequency of items in the dictionary</returns>
        private static int GetTotalFrequency<T>(Dictionary<T, ObjectFrequency<T>> dictionary)
        {
            return dictionary.Values.ToList().Select(v => v.Frequency)
                .Aggregate(0, (current, item) => current + item);
        }
    }
}