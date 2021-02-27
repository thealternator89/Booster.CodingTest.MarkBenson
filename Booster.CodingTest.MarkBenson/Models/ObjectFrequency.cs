namespace Booster.CodingTest.MarkBenson.Models
{
    public class ObjectFrequency<T>
    {
        public ObjectFrequency(T key)
        {
            Key = key;
        }

        public T Key { get; }
        public int Frequency { get; set; } = 1;

        public override string ToString()
        {
            return $"{Key} ({Frequency})";
        }
    }
}