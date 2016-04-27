using System;

namespace BenchmarkIt
{
    public class Against
    {
        private readonly Benchmark benchmark;

        internal Against(Benchmark benchmark)
        {
            this.benchmark = benchmark;
        }

        public Benchmark This(string label, Action function)
        {
            benchmark.Add(label, function);
            return benchmark;
        }
    }
}
