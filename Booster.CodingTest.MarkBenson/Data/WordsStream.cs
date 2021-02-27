using System;
using System.IO;
using System.Text;
using Booster.CodingTest.Library;

namespace Booster.CodingTest.MarkBenson.Data
{
    public sealed class WordsStream : IDisposable
    {
        private readonly StreamReader _reader = new(new WordStream());
        
        private bool _isDisposed;

        public string ReadWord()
        {
            var builder = new StringBuilder();

            while (true)
            {
                var currentInt = _reader.Read();
                // Ignore NULL characters.
                if (currentInt == 0)
                {
                    continue;
                }

                var current = Convert.ToChar(currentInt);
                if (current == ' ' || current == '.')
                {
                    break;
                }

                builder.Append(current);
            }

            // If the current word isn't empty use it, otherwise try again.
            return builder.Length != 0 ? builder.ToString().ToLower() : ReadWord();
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _reader.Dispose();
                }
                _isDisposed = true;
            }
        }

        ~WordsStream()
        {
            Dispose();
        }

    }
}