using System;

namespace BenchmarkIt
{
	public class Constraint {
		private readonly Benchmark _benchmark;

		private BenchmarkType _type;
		
        internal BenchmarkType Type {
			get { return _type; }
		}

		internal Constraint(Benchmark benchmark) {
			_benchmark = benchmark;
		}

		/// <summary>
		/// Makes the benchmark use a number of iterations
		/// </summary>
		/// <value>The iterations.</value>
		public Benchmark Iterations {
			get {
				_type = BenchmarkType.Iterations;
				return _benchmark;
			}
		}

		/// <summary>
		/// Makes the benchmark run for a number of seconds
		/// </summary>
		/// <value>The benchmark</value>
		public Benchmark Seconds {
			get {
				_type = BenchmarkType.Seconds;
				return _benchmark;
			}
		}

		/// <summary>
		/// Makes the benchmark run for a number of minutes
		/// </summary>
		/// <value>The benchmark</value>
		public Benchmark Minutes {
			get {
				_type = BenchmarkType.Minutes;
				return _benchmark;
			}
		}

		/// <summary>
		/// Makes the benchmark run for a number of hours
		/// </summary>
		/// <value>The benchmark</value>
		public Benchmark Hours {
			get {
				_type = BenchmarkType.Hours;
				return _benchmark;
			}
		}
	}
}

