using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BenchmarkIt
{
    public class Result
    {
        private string _label;
        private readonly BenchmarkType _type;

        private static readonly ResultColumn[] _timeResultColumns = new ResultColumn[]
	    {
	        new ResultColumn(header: "Name", width: 40),
            new ResultColumn(header: "Iterations", width: 20),
            new ResultColumn(header: "Percent", width: 30), 
	    };

        private static readonly ResultColumn[] _iterationResultColumns = new ResultColumn[]
	    {
	        new ResultColumn(header: "Name", width: 40),
            new ResultColumn(header: "Milliseconds", width: 20),
            new ResultColumn(header: "Percent", width: 30), 
	    };

        public Result(string label, BenchmarkType type)
        {
            _label = label;
            _type = type;
        }

        /// <summary>
        /// Gets the stopwatch used to time the benchmark
        /// </summary>
        public Stopwatch Stopwatch { get; internal set; }

        /// <summary>
        /// Gets the total iterations that occured during the benchmark
        /// </summary>
        public int TotalIterations { get; internal set; }

        /// <summary>
        /// Gets a string containing some formated stats about the result
        /// </summary>
        public string Stats
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append("-----");
                sb.Append(_label);
                sb.AppendLine("-----");
                sb.AppendLine("Total time: " + Stopwatch.Elapsed);
                sb.AppendLine("Ticks per execute: " + Stopwatch.ElapsedTicks / TotalIterations);
                sb.AppendLine("Milliseconds per execute: " + Stopwatch.ElapsedMilliseconds / TotalIterations);

                return sb.ToString();
            }
        }

        public void PrintStats()
        {
            var originalColor = Console.ForegroundColor;

            Console.Write("----- Stats for ");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(_label);
            Console.ForegroundColor = originalColor;
            Console.WriteLine("-----");

            Console.Write("Total time: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Stopwatch.Elapsed);
            Console.ForegroundColor = originalColor;

            Console.Write("Ticks per execute: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Stopwatch.ElapsedTicks / TotalIterations);
            Console.ForegroundColor = originalColor;

            Console.Write("Milliseconds per execute: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Milliseconds per execute: " + Stopwatch.ElapsedMilliseconds / TotalIterations);
            Console.ForegroundColor = originalColor;
        }

        /// <summary>
        /// Compares this result against one or more other results and returns a string with information
        /// </summary>
        /// <param name="results">Other results</param>
        public void Compare(params Result[] results)
        {
            var mergedResults = results.ToList();
            mergedResults.Insert(0, this);
            PrintComparison(mergedResults.ToArray());
        }

        /// <summary>
        /// Compares the given results against one another
        /// </summary>
        /// <param name="results"></param>
        public static void PrintComparison(params Result[] results)
        {
            if (results[0]._type == BenchmarkType.Iterations)
            {
                PrintComparisonIterations(results);
            }
            else
            {
                PrintComparisonTime(results);
            }
        }

        private static void PrintComparisonTime(Result[] results)
        {
            var type = results[0]._type;

            Result leastIterations = results[0], mostIterations = results[0];
            for (int i = 0; i < results.Length; i++)
            {
                var result = results[i];
                if (result._type != type)
                {
                    throw new ArgumentException(
                        "Can not compare results that are of a different metric (i.e. one time based, another iteration based)");
                }

                // we are intrested in iterations
                if (leastIterations.TotalIterations > result.TotalIterations)
                {
                    leastIterations = result;
                }
                else if (mostIterations.TotalIterations < result.TotalIterations)
                {
                    mostIterations = result;
                }
            }

            var originalColor = Console.ForegroundColor;

            // header
            var header = string.Join("", _timeResultColumns.Select(c => c.Header.PadRight(c.Width)));
            Console.WriteLine(header);

            // results columns
            for (int i = 0; i < results.Length; i++)
            {
                var result = results[i];

                Console.ForegroundColor = ConsoleColor.Red;
                if (result.TotalIterations == mostIterations.TotalIterations)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                var percentLess = Math.Round((double)result.TotalIterations / (double)mostIterations.TotalIterations * 100.0d, 1);

                var outputColumns = new object[]
                {
                    result._label,
                    result.TotalIterations,
                    percentLess + "%"
                };

                for (int c = 0; c < outputColumns.Length; c++)
                {
                    Console.Write(outputColumns[c].ToString().PadRight(_timeResultColumns[c].Width));
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = originalColor;
        }

        private static void PrintComparisonIterations(Result[] results)
        {
            var type = results[0]._type;

            Result lowestTime = results[0], highestTime = results[0];
            for (int i = 0; i < results.Length; i++)
            {
                var result = results[i];
                if (result._type != type)
                {
                    throw new ArgumentException(
                        "Can not compare results that are of a different metric (i.e. one time based, another iteration based)");
                }

                if (string.IsNullOrEmpty(result._label))
                {
                    result._label = "Result " + (i + 1);
                }

                // we are intrested in iterations
                if (lowestTime.Stopwatch.ElapsedTicks > result.Stopwatch.ElapsedTicks)
                {
                    lowestTime = result;
                }
                else if (highestTime.Stopwatch.ElapsedTicks < result.Stopwatch.ElapsedTicks)
                {
                    highestTime = result;
                }
            }

            var originalColor = Console.ForegroundColor;

            // header
            var header = string.Join("", _iterationResultColumns.Select(c => c.Header.PadRight(c.Width)));
            Console.WriteLine(header);

            // results columns
            foreach (var result in results)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (result.Stopwatch.ElapsedTicks == lowestTime.Stopwatch.ElapsedTicks)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                var percentLess = Math.Round((double)result.Stopwatch.ElapsedTicks / (double)lowestTime.Stopwatch.ElapsedTicks * 100.0d, 1);

                var outputColumns = new object[]
                {
                    result._label,
                    result.Stopwatch.ElapsedMilliseconds,
                    percentLess + "%"
                };

                for (int c = 0; c < outputColumns.Length; c++)
                {
                    Console.Write(outputColumns[c].ToString().PadRight(_iterationResultColumns[c].Width));
                }
                Console.Write("\n");
            }
            Console.ForegroundColor = originalColor;
        }
    }
}

