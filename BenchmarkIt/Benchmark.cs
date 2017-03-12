using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BenchmarkIt
{
    /// <summary>
    /// Provides benchmarks of functions
    /// </summary>
    public class Benchmark
    {
        public static JustClause Just
        {
            get
            {
                return new JustClause();
            }
        }

        public static ThisClause This(Action action, string label = null)
        {
            var benchmark = new Benchmark();
            benchmark.Add(action, label);
            return new ThisClause(benchmark);
        }

        public static BenchmarkResults These((Action, string)[] actions)
        {
            var benchmark = new Benchmark();
            foreach (var action in actions)
            {
                benchmark.Add(action.Item1, action.Item2);
            }
            return benchmark.Run();
        }

        public static BenchmarkSettings Settings { get; } = new BenchmarkSettings();

        private List<(Action action, string label)> actions = new List<(Action, string)>();
        private BenchmarkSettings settings;

        public Benchmark()
        {
            settings = Settings;
        }

        public Benchmark(BenchmarkSettings settings)
        {
            this.settings = settings;
        }

        public BenchmarkResults Run()
        {
            var results = actions.Select(x => RunAction(x.action, x.label));
            return new BenchmarkResults(results.ToArray());
        }

        private Result RunAction(Action action, string label)
        {
            // http://monsur.hossa.in/2012/12/11/benchmarkjs.html
            // http://ejohn.org/blog/javascript-benchmark-quality/
            // http://ejohn.org/apps/measure/
            var (batchSize, batchTime) = CalculateBatchSize(action);

            double error = double.MaxValue;
            var runTicks = new List<long>();
            var runs = new List<Run>();
            var start = DateTime.Now;

            // Always run it 2 times at least so we can calculate the standard deviation
            while ((error > settings.MinimumErrorToAccept || runTicks.Count <= 2))
            {
                // Give the test as good a chance as possible of avoiding garbage collection
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                var sw = Stopwatch.StartNew();
                for (var i = 0; i < batchSize; i++)
                {
                    action();
                }
                sw.Stop();

                runTicks.Add(sw.ElapsedTicks);
                var runStatistics = CalculateRunStatistics(runTicks, batchSize);
                error = runStatistics.Error;
                
                if ((DateTime.Now - start).TotalMilliseconds > settings.MaxTime)
                {
                    runStatistics.ExceededMaxTime = true;
                    runs.Add(runStatistics);
                    break;
                }

                runs.Add(runStatistics);
                Console.WriteLine($"Error: {error}");
            }

            Console.WriteLine($"Time ran for: {(DateTime.Now - start).TotalMilliseconds}");

            var result = new Result()
            {
                Label = label,
                BatchSize = batchSize,
                BatchTime = batchTime,
                Runs = runs
            };
            return result;
        }
                
        private Run CalculateRunStatistics(List<long> values, int batchSize)
        {
            // Only use the last 10 values
            var valuesToUse = values.Skip(Math.Max(0, values.Count() - settings.BatchesToWorkAcross)).ToArray();
            var ticks = values.Last();

            // TODO: Not shitty implementation, use a rolling average and standard deviation
            double mean = valuesToUse.Average();
            double variance = valuesToUse.Average(v => Math.Pow(v - mean, 2));
            double deviation = Math.Sqrt(variance);

            double msSpentOnRun = ((double)ticks / (double)Stopwatch.Frequency) * 1000d;
            double upgradeFactor = 1000 / msSpentOnRun;
            double operationsPerSecond = batchSize * upgradeFactor;

            int currTDistributionValue = Math.Min(settings.BatchesToWorkAcross, values.Count());
            double standardErrorsMean = deviation / Math.Sqrt(valuesToUse.Count()) * TDistribution.Values[currTDistributionValue];
            double error = Math.Max((standardErrorsMean / mean) * 100, 0);

            return new Run()
            {
                MeanTicks = mean,
                StandardDeviation = deviation,
                Variance = variance,
                StandardErrorsMean = standardErrorsMean,
                Error = error,
                OperationsPerSecond = operationsPerSecond,
                Ticks = ticks
            };
        }

        /// <summary>
        /// Calculates how many times we should run our action per batch
        /// </summary>
        private (int batchSize, int batchTime) CalculateBatchSize(Action action)
        {
            int count = 0;
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < settings.BatchSizeTime)
            {
                action();
                count++;
            }

            // TODO: Calculate a good batch size based on how long we're taking to run
            // So slow running actions don't need to be run many times, but fast might need 
            // running a lot for a shorter period of time
            return (count, settings.BatchSizeTime);
        }

        /// <summary>
        /// Add an action to benchmark
        /// </summary>
        /// <example>Add(() => Math.Sqrt(1.23), "Sqare root benchmark")</example>
        /// <param name="action">The method to benchmark</param>
        /// <param name="label">A name for this benchmark</param>
        public void Add(Action action, string label = null)
        {
            actions.Add(
                (action,
                label ?? $"Test {actions.Count + 1}"));
        }
    }


}

