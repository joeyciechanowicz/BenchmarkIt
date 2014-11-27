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
        private readonly List<Tuple<string, Action>> _functions;

        private int _amount;
        private int _warmup;

        private readonly Constraint _constraint;
        private readonly Against _against;


        private Benchmark(Action function, string label)
        {
            _functions = new List<Tuple<string, Action>>();
            _constraint = new Constraint(this);
            _against = new Against(this);

            _functions.Add(new Tuple<string, Action>(label, function));
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
            get { return _against; }
        }

        /// <summary>
        /// Specify the amount that the benchmark should be ran for (i.e. iterations, minutes etc)
        /// </summary>
        /// <param name="amount">Amount.</param>
        public Constraint For(int amount)
        {
            _amount = amount;
            return _constraint;
        }

        /// <summary>
        /// Makes the benchmark run the function n times before starting the benchmark
        /// </summary>
        /// <param name="n">The number of iterations to run of the function to warmup</param> 
        /// <value>The benchmark</value>
        public Benchmark WithWarmup(int n)
        {
            if (n < 0)
            {
                throw new ArgumentException("The number of warmup iterations can not be negative");
            }
            _warmup = n;
            return this;
        }

        /// <summary>
        /// Runs the benchmark and returns the result
        /// </summary>
        internal Result[] Run()
        {
            switch (_constraint.Type)
            {
                case BenchmarkType.Seconds:
                    return RunForTime(DateTime.Now.AddSeconds(_amount));
                case BenchmarkType.Minutes:
                    return RunForTime(DateTime.Now.AddMinutes(_amount));
                case BenchmarkType.Hours:
                    return RunForTime(DateTime.Now.AddHours(_amount));
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
            _functions.Add(new Tuple<string, Action>(label, function));
        }

        private Result[] RunIterations()
        {
            var results = new Result[_functions.Count];

            // loop through our function
            for (int f = 0; f < _functions.Count; f++)
            {
                var function = _functions[f].Item2;

                for (int i = 0; i < _warmup; i++)
                {
                    function();
                }

                // benchmark them
                Stopwatch sw = Stopwatch.StartNew();
                for (int i = 0; i < this._amount; i++)
                {
                    function();
                }
                sw.Stop();

                var result = new Result(_functions[f].Item1, _constraint.Type)
                {
                    Stopwatch = sw,
                    TotalIterations = _amount
                };

                results[f] = result;
            }
            return results;
        }

        private Result[] RunForTime(DateTime until)
        {
            var results = new Result[_functions.Count];

            // loop through our function
            for (int f = 0; f < _functions.Count; f++)
            {
                var function = _functions[f].Item2;

                for (int i = 0; i < _warmup; i++)
                {
                    function();
                }

                var timeUntilStop = until - DateTime.Now;

                // run the function until we hit the desired time
                int count = 0;
                Stopwatch sw = Stopwatch.StartNew();
                while (sw.Elapsed <= timeUntilStop)
                {
                    count++;
                    function();
                }
                sw.Stop();

                var result = new Result(_functions[f].Item1, _constraint.Type)
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

