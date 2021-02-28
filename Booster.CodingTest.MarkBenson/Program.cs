using System;
using Booster.CodingTest.MarkBenson.Controller;

namespace Booster.CodingTest.MarkBenson
{
    class Program
    {
        static void Main(string[] args)
        {
            var sleepTime = 0;

            if (args.Length == 1)
            {
                var parsed = int.TryParse(args[0], out sleepTime);
                if (!parsed)
                {
                    Console.WriteLine("WARN: Argument passed is non-numeric. Pass a numeric argument to reduce processing speed by sleeping the thread.");
                }
            }

            ConsoleController.Run(sleepTime);
        }
    }
}