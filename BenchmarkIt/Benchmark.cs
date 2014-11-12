using System;
using System.Diagnostics;

namespace BenchmarkIt
{
	/// <summary>
	/// Provides benchmarks of functions
	/// </summary>
	public class Benchmark
	{
        private readonly Action _function;
		private int _amount;
		private int _cacheWarmup = 0;
		private Constraint _constraint;

		private Benchmark(Action function)
		{
		    _function = function;
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
			_amount = amount;
			_constraint = new Constraint (this);
			return _constraint;
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
			this._cacheWarmup = n;
			return this;
		}

		/// <summary>
		/// Runs the benchmark and returns the result
		/// </summary>
		public Result Run (string label = null)
		{
		    switch (_constraint.Type)
		    {
		        case BenchmarkType.Seconds:
		            return RunForTime(label, DateTime.Now.AddSeconds(_amount));
		        case BenchmarkType.Minutes:
		            return RunForTime(label, DateTime.Now.AddMinutes(_amount));
		        case BenchmarkType.Hours:
		            return RunForTime(label, DateTime.Now.AddHours(_amount));
		        case BenchmarkType.Iterations:
		            return RunIterations(label);
                default:
                    throw new ArgumentException("Unsupported run type: " + _constraint.Type);
		    }
		}

		private Result RunIterations(string label) {

			for (int i = 0; i < _cacheWarmup; i++) {
				_function ();
			}

			Stopwatch sw = Stopwatch.StartNew ();
			for (int i = 0; i < this._amount; i++) {
				_function ();
			}
			sw.Stop ();

            var result = new Result(label, _constraint.Type);
			result.Stopwatch = sw;
			result.TotalIterations = _amount;

			return result;
		}

		private Result RunForTime(string label, DateTime until) {
			for (int i = 0; i < _cacheWarmup; i++) {
				_function ();
			}

			int count = 0;
			Stopwatch sw = Stopwatch.StartNew ();
			while (DateTime.Now < until) {
				count++;
				_function ();
			}

			sw.Stop ();

			var result = new Result (label, _constraint.Type);
			result.Stopwatch = sw;
			result.TotalIterations = count;

			return result;
		}
	}


}

