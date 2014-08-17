using System;
using System.Diagnostics;

namespace BenchmarkIt
{
	/// <summary>
	/// Provides benchmarks of functions
	/// </summary>
	public class Benchmark
	{
		private Action function;
		private int amount;
		private BenchmarkType type;
		private int cacheWarmup = 0;
		private Constraint constraint;

		private Benchmark(Action function) {
			this.function = function;
		}

		/// <summary>
		/// Create a new benchmark
		/// </summary>
		/// <param name="function">Function to be benchmarked</param>
		public static Benchmark This(Action function) {
			return new Benchmark(function);
		}

		/// <summary>
		/// Specify the amount that the benchmark should be ran for (i.e. iterations, minutes etc)
		/// </summary>
		/// <param name="amount">Amount.</param>
		public Constraint For(int amount) {
			this.amount = amount;
			this.constraint = new Constraint (this);
			return this.constraint;
		}

		/// <summary>
		/// Makes the benchmark run the fucntion n times before starting the benchmark
		/// </summary>
		/// <param name="n">The number of iterations to run of the function to warmup</param> 
		/// <value>The benchmark</value>
		public Benchmark WithCacheWarmup(int n) {
			if (n < 0) {
				throw new ArgumentException ("The number of warmup iterations can not be negative");
			}
			this.cacheWarmup = n;
			return this;
		}

		/// <summary>
		/// Runs the benchmark and returns the result
		/// </summary>
		public Result Run ()
		{
			if (type == BenchmarkType.Iterations) {
				return RunIterations ();
			} else {
				DateTime until;
				switch (type) {
					case BenchmarkType.Seconds:
						until = DateTime.Now.AddSeconds (amount);
						break;
					case BenchmarkType.Minutes:
						until = DateTime.Now.AddMinutes (amount);
						break;
					case BenchmarkType.Hours:
						until = DateTime.Now.AddHours (amount);
						break;
				}
				return RunForTime (until);
			}
		}

		private Result RunIterations() {

			for (int i = 0; i < cacheWarmup; i++) {
				function ();
			}

			Stopwatch sw = Stopwatch.StartNew ();
			for (int i = 0; i < this.amount; i++) {
				function ();
			}
			sw.Stop ();

			var result = new Result ();
			result.Stopwatch = sw;
			result.TotalIterations = amount;

			return result;
		}

		private Result RunForTime(DateTime until) {
			for (int i = 0; i < cacheWarmup; i++) {
				function ();
			}

			int count = 0;
			Stopwatch sw = Stopwatch.StartNew ();
			while (DateTime.Now < until) {
				count++;
				function ();
			}

			sw.Stop ();

			var result = new Result ();
			result.Stopwatch = sw;
			result.TotalIterations = count;

			return result;
		}
	}


}

