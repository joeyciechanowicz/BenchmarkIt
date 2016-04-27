using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BenchmarkIt
{
    /// <summary>
    /// Provides benchmarks of functions
    /// </summary>
    public class Benchmark
    {
        private struct ActionPair
        {
            public string Name { get; private set; }
            public Action Action { get; private set; }

            public ActionPair(string name, Action action) : this()
            {
                Name = name;
                Action = action;
            }
        }

        private readonly List<ActionPair> actions;

        private int benchmarkLength;
        private int warmup = 1;

        private readonly Constraint constraint;
        private readonly Against against;


        private Benchmark(Action function, string label)
        {
            actions = new List<ActionPair>();
            constraint = new Constraint(this);
            against = new Against(this);

            actions.Add(new ActionPair(label, function));
        }

        /// <summary>
        /// Create a new benchmark
        /// </summary>
        /// <param name="function">Function to be benchmarked</param>
        /// <param name="label">Label to give the result</param>
        public static Benchmark This(string label, Action function)
        {
            return new Benchmark(function, label);
        }

        /// <summary>
        /// Add another function to benchmark against
        /// </summary>
        public Against Against
        {
            get { return against; }
        }

        /// <summary>
        /// Specify the amount that the benchmark should be ran for (i.e. iterations, minutes etc)
        /// </summary>
        /// <param name="length">Amount.</param>
        public Constraint For(int length)
        {
            benchmarkLength = length;
            return constraint;
        }

        /// <summary>
        /// Makes the benchmark run the function n times before starting the benchmark
        /// </summary>
        /// <param name="n">The number of iterations to run of the function to warmup</param> 
        /// <value>The benchmark</value>
        public Benchmark WithWarmup(int n)
        {
            if (n < 1)
            {
                throw new ArgumentException("The number of warmup iterations can not be less than one");
            }
            warmup = n;
            return this;
        }

        /// <summary>
        /// Runs the benchmark and returns the result
        /// </summary>
        internal Result[] Run()
        {
            switch (constraint.Type)
            {
                case BenchmarkType.Seconds:
                    return RunForTime(TimeSpan.FromSeconds(benchmarkLength));
                case BenchmarkType.Minutes:
                    return RunForTime(TimeSpan.FromSeconds(benchmarkLength));
                case BenchmarkType.Hours:
                    return RunForTime(TimeSpan.FromSeconds(benchmarkLength));
                default:
                    return RunIterations();
            }
        }

        /// <summary>
        /// Adds another function to benchmark
        /// </summary>
        /// <param name="label"></param>
        /// <param name="function"></param>
        internal void Add(string label, Action function)
        {
            actions.Add(new ActionPair(label, function));
        }

        private Result[] RunIterations()
        {
            var results = new Result[actions.Count];

            // loop through our function
            for (int f = 0; f < actions.Count; f++)
            {
                var action = actions[f].Action;

                // Always warmup at least once
                action();
                for (int i = 1; i < warmup; i++)
                {
                    action();
                }

                var sw = new Stopwatch();

                // Give the test as good a chance as possible of avoiding garbage collection
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                // benchmark them
                sw.Start();
                for (int i = 0; i < benchmarkLength; i++)
                {
                    action();
                }
                sw.Stop();

                var result = new Result(actions[f].Name, constraint.Type)
                {
                    Stopwatch = sw,
                    TotalIterations = benchmarkLength
                };

                results[f] = result;
            }
            return results;
        }

        private Result[] RunForTime(TimeSpan amount)
        {
            var results = new Result[actions.Count];

            // loop through our function
            for (int f = 0; f < actions.Count; f++)
            {
                var function = actions[f].Action;

                for (int i = 0; i < warmup; i++)
                {
                    function();
                }

                Stopwatch sw = new Stopwatch();

                // Give the test as good a chance as possible of avoiding garbage collection
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                // run the function until we hit the desired time
                sw.Start();
                int count = 0;
                while (sw.Elapsed.Ticks <= amount.Ticks)
                {
                    count++;
                    function();
                }
                sw.Stop();

                var result = new Result(actions[f].Name, constraint.Type)
                {
                    Stopwatch = sw,
                    TotalIterations = count
                };

                results[f] = result;
            }

            return results;
        }
    }


}

