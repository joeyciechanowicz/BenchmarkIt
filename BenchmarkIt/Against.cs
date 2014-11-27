using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BenchmarkIt
{
    public class Against
    {
        private readonly Benchmark _benchmark;

        internal Against(Benchmark benchmark)
        {
            _benchmark = benchmark;
        }

        public Benchmark This(string label, Action function)
        {
            _benchmark.Add(label, function);
            return _benchmark;
        }
    }
}
