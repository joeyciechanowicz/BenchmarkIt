using System;

namespace BenchmarkIt
{
	public class Constraint {
		private Benchmark benchmark;

		private BenchmarkType type;
		internal BenchmarkType Type {
			get { return type; }
		}

		internal Constraint(Benchmark benchmark) {
			this.benchmark = benchmark;
		}

		/// <summary>
		/// Makes the benchmark use a number of iterations
		/// </summary>
		/// <value>The iterations.</value>
		public Benchmark Iterations {
			get {
				this.type = BenchmarkType.Iterations;
				return this.benchmark;
			}
		}

		/// <summary>
		/// Makes the benchmark run for a number of seconds
		/// </summary>
		/// <value>The benchmark</value>
		public Benchmark Seconds {
			get {
				this.type = BenchmarkType.Seconds;
				return this.benchmark;
			}
		}

		/// <summary>
		/// Makes the benchmark run for a number of minutes
		/// </summary>
		/// <value>The benchmark</value>
		public Benchmark Minutes {
			get {
				this.type = BenchmarkType.Minutes;
				return this.benchmark;
			}
		}

		/// <summary>
		/// Makes the benchmark run for a number of hours
		/// </summary>
		/// <value>The benchmark</value>
		public Benchmark Hours {
			get {
				this.type = BenchmarkType.Hours;
				return this.benchmark;
			}
		}
	}
}

