using Booster.CodingTest.MarkBenson.Models;
using Xunit;

namespace Booster.CodingTest.MarkBenson.UnitTests.Models
{
    public class ObjectFrequencyTest
    {
        [Fact]
        public void String_BuildsStringCorrectly()
        {
            AssertObjectFrequencyToString("string (103)", new ObjectFrequency<string>("string") {Frequency = 103});
            AssertObjectFrequencyToString("test (2147483647)", new ObjectFrequency<string>("test") {Frequency = int.MaxValue});
        }

        [Fact]
        public void Char_BuildsStringCorrectly()
        {
            AssertObjectFrequencyToString("p (5)", new ObjectFrequency<char>('p') {Frequency = 5});
            AssertObjectFrequencyToString("z (2)", new ObjectFrequency<char>('z') {Frequency = 2});
        }

        [Fact]
        public void Other_BuildsStringCorrectly()
        {
            AssertObjectFrequencyToString("17 (51)", new ObjectFrequency<int>(17) {Frequency = 51});
            AssertObjectFrequencyToString("3.14 (7)", new ObjectFrequency<float>(3.14F) {Frequency = 7});
            AssertObjectFrequencyToString("System.Object (1)", new ObjectFrequency<object>(new object()));
        }

        private static void AssertObjectFrequencyToString<T>(string expected, ObjectFrequency<T> obj)
        {
            Assert.Equal(expected, obj.ToString());
        }
    }
}