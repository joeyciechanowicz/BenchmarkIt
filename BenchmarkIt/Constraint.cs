using System;

namespace BenchmarkIt
{
    /// <summary>
    /// Running constraint to be used for a benchmark
    /// </summary>
    public class Constraint
    {
        private readonly Benchmark _benchmark;

        private BenchmarkType _type = BenchmarkType.Iterations;

        internal BenchmarkType Type
        {
            get { return _type; }
        }

        internal Constraint(Benchmark benchmark)
        {
            _benchmark = benchmark;
        }

        /// <summary>
        /// Makes the benchmark use a number of iterations
        /// </summary>
        /// <value>The iterations.</value>
        public Result[] Iterations()
        {
            _type = BenchmarkType.Iterations;
            return _benchmark.Run();
        }

        /// <summary>
        /// Makes the benchmark run for a number of seconds
        /// </summary>
        /// <value>The benchmark</value>
        public Result[] Seconds()
        {
            _type = BenchmarkType.Seconds;
            return _benchmark.Run();
        }

        /// <summary>
        /// Makes the benchmark run for a number of minutes
        /// </summary>
        /// <value>The benchmark</value>
        public Result[] Minutes()
        {
            _type = BenchmarkType.Minutes;
            return _benchmark.Run();
        }

        /// <summary>
        /// Makes the benchmark run for a number of hours
        /// </summary>
        /// <value>The benchmark</value>
        public Result[] Hours()
        {
            _type = BenchmarkType.Hours;
            return _benchmark.Run();
        }
    }
}

