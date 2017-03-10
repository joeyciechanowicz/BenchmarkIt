using System;

namespace BenchmarkIt
{
    public class ThisClause
    {
        private Benchmark benchmark;

        public ThisClause(Benchmark benchmark)
        {
            this.benchmark = benchmark;
        }

        public BenchmarkResults Against(Action action, string label = null)
        {
            benchmark.Add(action, label);
            return benchmark.Run();
        }
    }
}