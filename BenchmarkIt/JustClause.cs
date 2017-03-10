using System;

namespace BenchmarkIt
{
    public class JustClause
    {
        public BenchmarkResults This(Action action, string label = null)
        {
            var benchmark = new Benchmark();
            benchmark.Add(action, label);
            return benchmark.Run();
        }
    }
}